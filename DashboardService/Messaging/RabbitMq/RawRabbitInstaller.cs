using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.Configuration.Exchange;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;

namespace DashboardService.Messaging.RabbitMq;

public static class RawRabbitInstaller
{
    public static IServiceCollection AddRabbitListeners(this IServiceCollection services, RabbitMqOptions options)
    {
        services.AddRawRabbit(new RawRabbitOptions
        {
            ClientConfiguration = new RawRabbitConfiguration
            {
                Username = "guest",
                Password = "guest",
                VirtualHost = "/",
                Port = options.Port,
                Hostnames = new List<string> { options.Host },
                RequestTimeout = TimeSpan.FromSeconds(10),
                PublishConfirmTimeout = TimeSpan.FromSeconds(1),
                RecoveryInterval = TimeSpan.FromSeconds(1),
                PersistentDeliveryMode = true,
                AutoCloseConnection = true,
                AutomaticRecovery = true,
                TopologyRecovery = true,
                Exchange = new GeneralExchangeConfiguration
                {
                    Durable = true,
                    AutoDelete = false,
                    Type = ExchangeType.Topic
                },
                Queue = new GeneralQueueConfiguration
                {
                    Durable = true,
                    AutoDelete = false,
                    Exclusive = false
                }
            }
        });

        services.AddSingleton(svc => new RabbitEventListener(svc.GetRequiredService<IBusClient>(), svc));

        return services;
    }
}

public static class RabbitListenersInstaller
{
    public static void UseRabbitListeners(this IApplicationBuilder app, List<Type> eventTypes)
    {
        app.ApplicationServices.GetRequiredService<RabbitEventListener>().ListenTo(eventTypes);
    }
}