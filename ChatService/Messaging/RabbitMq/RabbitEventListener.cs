using System;
using System.Collections.Generic;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration.Exchange;

namespace ChatService.Messaging.RabbitMq;

public class RabbitEventListener
{
    private readonly IBusClient busClient;
    private readonly IServiceProvider serviceProvider;

    public RabbitEventListener(
        IBusClient busClient,
        IServiceProvider serviceProvider)
    {
        this.busClient = busClient;
        this.serviceProvider = serviceProvider;
    }

    public void ListenTo(List<Type> eventsToSubscribe)
    {
        foreach (var evtType in eventsToSubscribe)
            //add check if is INotification
            GetType()
                .GetMethod("Subscribe",
                    BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(evtType)
                .Invoke(this, new object[] { });
    }

    private void Subscribe<T>() where T : INotification
    {
        //TODO: move exchange name and queue prefix to cfg
        busClient.SubscribeAsync<T>(
            async msg =>
            {
                //add logging
                using (var scope = serviceProvider.CreateScope())
                {
                    var internalBus = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await internalBus.Publish(msg);
                }
            },
            cfg => cfg.UseSubscribeConfiguration(
                c => c
                    .OnDeclaredExchange(e => e
                        .WithName("lab-dotnet-micro")
                        .WithType(ExchangeType.Topic)
                        .WithArgument("key", typeof(T).Name.ToLower()))
                    .FromDeclaredQueue(q => q.WithName("lab-chat-service-" + typeof(T).Name)))
        );
    }
}