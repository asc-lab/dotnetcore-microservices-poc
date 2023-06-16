using System;
using System.Collections.Generic;
using System.Reflection;
using EasyNetQ;
using EasyNetQ.Topology;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PolicySearchService.Messaging.RabbitMq;

public class RabbitEventListener
{
    private readonly IBus busClient;
    private readonly IServiceProvider serviceProvider;
    private readonly Exchange exchange;

    public RabbitEventListener(
        IBus busClient,
        IServiceProvider serviceProvider)
    {
        this.busClient = busClient;
        this.serviceProvider = serviceProvider;
        this.exchange = this.busClient.Advanced.ExchangeDeclare("lab-dotnet-micro", ExchangeType.Topic);
    }

    public void ListenTo(List<Type> eventsToSubscribe)
    {
        foreach (var evtType in eventsToSubscribe)
            //add check if is INotification
            GetType()
                .GetMethod("Subscribe", BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(evtType)
                .Invoke(this, new object[] { });
    }

    private void Subscribe<T>() where T : INotification
    {
        //create queue & binding
        var queue = busClient.Advanced.QueueDeclare("lab-policy-search-service-" + typeof(T).Name);
        busClient.Advanced.Bind(exchange, queue, typeof(T).Name.ToLower());
        //subscribe
        busClient.Advanced.Consume(queue, (IMessage<T> msg, MessageReceivedInfo messageReceivedInfo) =>
        {
            using var scope = serviceProvider.CreateScope();
            var internalBus = scope.ServiceProvider.GetRequiredService<IMediator>();
            return internalBus.Publish(msg.Body);
        });
    }
}