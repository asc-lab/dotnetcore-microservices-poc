using PolicyService.Api.Events;
using RawRabbit;
using RawRabbit.Operations.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.Messaging.RabbitMq
{
    public class RabbitEventPublisher : IEventPublisher
    {
        private readonly IBusClient busClient;

        public RabbitEventPublisher(IBusClient busClient)
        {
            this.busClient = busClient;
        }

        public Task PublishMessage<T>(T msg)
        {
            return busClient.BasicPublishAsync(msg, cfg => {
                cfg.OnExchange("lab-dotnet-micro").WithRoutingKey(typeof(T).Name.ToLower());
            });
        }
    }
}
