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
        private readonly ILogger logger;

        public Outbox(IBusClient busClient, ISessionFactory sessionFactory, ILogger logger)
        {
            this.busClient = busClient;
            this.sessionFactory = sessionFactory;
            this.logger = logger;
        }


        public void PushPendingMessages()
        {
            var messagesToPush = FetchPendingMessages();

            foreach (var msg in messagesToPush)
            {
                TryPush(msg);
            }
        }

        private IEnumerable<Message> FetchPendingMessages()
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

        private void TryPush(Message msg)
        {
            TryInTx(session =>
            {
                PublishMessage(msg);
                    
                session
                    .CreateQuery("delete Message where id=:id")
                    .SetParameter("id", msg.Id)
                    .ExecuteUpdate();
            });
        }

        private void TryInTx(Action<IStatelessSession> action)
        {
            using (var session = sessionFactory.OpenStatelessSession())
            {
                var tx = session.BeginTransaction();
                try
                {
                    action(session);
                    tx.Commit();
                }
                catch (Exception e)
                {
                    logger.LogError(e,"Failed to push message from outbox",null);
                    tx?.Rollback();
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
}