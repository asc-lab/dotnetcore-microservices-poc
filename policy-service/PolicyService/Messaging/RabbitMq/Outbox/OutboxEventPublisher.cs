using System.Threading.Tasks;
using NHibernate;

namespace PolicyService.Messaging.RabbitMq.Outbox
{
    public class OutboxEventPublisher : IEventPublisher
    {
        private readonly ISession session;

        public OutboxEventPublisher(ISession session)
        {
            this.session = session;
        }

        public async Task PublishMessage<T>(T msg)
        {
            await session.SaveAsync(new Message(msg));
        }
    }
}