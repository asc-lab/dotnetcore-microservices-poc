using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using NHibernate;
using RawRabbit;

namespace PolicyService.Messaging.RabbitMq.Outbox
{
    public class Outbox
    {
        private readonly IBusClient busClient;
        private readonly ISessionFactory sessionFactory;
        private readonly OutboxLogger logger;

        public Outbox(IBusClient busClient, ISessionFactory sessionFactory, ILogger<Outbox> logger)
        {
            this.busClient = busClient;
            this.sessionFactory = sessionFactory;
            this.logger = new OutboxLogger(logger);
        }


        public void PushPendingMessages()
        {
            var messagesToPush = FetchPendingMessages();
            logger.LogPending(messagesToPush);

            foreach (var msg in messagesToPush)
            {
                if (!TryPush(msg))
                    break;
            }
        }

        private IList<Message> FetchPendingMessages()
        {
            List<Message> messagesToPush;
            using (var session = sessionFactory.OpenStatelessSession())
            {
                messagesToPush = session.Query<Message>()
                    .OrderBy(m => m.Id)
                    .Take(50)
                    .ToList();
            }

            return messagesToPush;
        }

        private bool TryPush(Message msg)
        {
            return TryInTx(session =>
            {
                PublishMessage(msg);
                    
                session
                    .CreateQuery("delete Message where id=:id")
                    .SetParameter("id", msg.Id)
                    .ExecuteUpdate();
            });
        }

        private bool TryInTx(Action<IStatelessSession> action)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                var tx = session.BeginTransaction();
                try
                {
                    action(session);
                    tx.Commit();
                    logger.LogSuccessPush();
                    return true;
                }
                catch (Exception e)
                {
                    logger.LogFailedPush(e);
                    tx?.Rollback();
                    return false;
                }
            }
        }

        private void PublishMessage(Message msg)
        {
            var deserializedMsg = msg.RecreateMessage();
            var messageKey = deserializedMsg.GetType().Name.ToLower();
            busClient.BasicPublishAsync(deserializedMsg,
                cfg =>
                {
                    cfg.OnExchange("lab-dotnet-micro").WithRoutingKey(messageKey);
                });
        }

    }

    class OutboxLogger
    {
        private readonly ILogger<Outbox> logger;

        public OutboxLogger(ILogger<Outbox> logger) => this.logger = logger;

        public void LogPending(IEnumerable<Message> messages)
        {
            logger.LogInformation($"{messages.Count()} messages about to be pushed.");
        }

        public void LogSuccessPush()
        {
            logger.LogInformation("Successfully pushed message");    
        }

        public void LogFailedPush(Exception e)
        {
            logger.LogError(e,"Failed to push message from outbox",null);
        }
    }
}