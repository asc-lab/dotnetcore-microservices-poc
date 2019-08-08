using System.Collections.Generic;
using System.Linq;
using NHibernate;
using RawRabbit;

namespace PolicyService.Messaging.RabbitMq.Outbox
{
    public class Outbox
    {
        private readonly IBusClient busClient;
        private readonly ISessionFactory sessionFactory;
        

        public Outbox(IBusClient busClient, ISessionFactory sessionFactory)
        {
            this.busClient = busClient;
            this.sessionFactory = sessionFactory;
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
            using (var session = sessionFactory.OpenSession())
            {
                using (var tx = session.BeginTransaction())
                {
                    var deserializedMsg = msg.RecreateMessage();
                    var messageKey = deserializedMsg.GetType().Name.ToLower();
                    busClient.BasicPublishAsync(deserializedMsg,
                        cfg =>
                        {
                            cfg.OnExchange("lab-dotnet-micro").WithRoutingKey(messageKey);
                        });
                    session
                        .CreateQuery("delete Message where id=:id")
                        .SetParameter("id", msg.Id)
                        .ExecuteUpdate();
                    tx.Commit();
                }
            }
        }

    }
}