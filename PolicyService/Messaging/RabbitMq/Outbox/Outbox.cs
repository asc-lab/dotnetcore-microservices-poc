using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyNetQ.Topology;
using Microsoft.Extensions.Logging;
using NHibernate;

namespace PolicyService.Messaging.RabbitMq.Outbox;

public class Outbox
{
    private readonly IBus busClient;
    private readonly OutboxLogger logger;
    private readonly ISessionFactory sessionFactory;
    private readonly Exchange exchange;

    public Outbox(IBus busClient, ISessionFactory sessionFactory, ILogger<Outbox> logger)
    {
        this.busClient = busClient;
        this.sessionFactory = sessionFactory;
        this.logger = new OutboxLogger(logger);
        this.exchange = busClient.Advanced.ExchangeDeclare("lab-dotnet-micro", ExchangeType.Topic);
    }


    public async Task PushPendingMessages()
    {
        var messagesToPush = FetchPendingMessages();
        logger.LogPending(messagesToPush);

        foreach (var msg in messagesToPush)
            if (!await TryPush(msg))
                break;
    }

    private IList<Message> FetchPendingMessages()
    {
        List<Message> messagesToPush;
        using var session = sessionFactory.OpenStatelessSession();
        messagesToPush = session.Query<Message>()
            .OrderBy(m => m.Id)
            .Take(50)
            .ToList();

        return messagesToPush;
    }

    private async Task<bool> TryPush(Message msg)
    {
        using var session = sessionFactory.OpenStatelessSession();
        var tx = session.BeginTransaction();
        try
        {
            await PublishMessage(msg);

            await session
                .CreateQuery("delete Message where id=:id")
                .SetParameter("id", msg.Id)
                .ExecuteUpdateAsync();

            await tx.CommitAsync();
            logger.LogSuccessPush();
            return true;
        }
        catch (Exception e)
        {
            logger.LogFailedPush(e);
            await tx?.RollbackAsync();
            return false;
        }
    }

    private async Task PublishMessage(Message msg)
    {
        var deserializedMsg = msg.RecreateMessage();
        var messageKey = deserializedMsg.GetType().Name.ToLower();
        await busClient.Advanced.PublishAsync(exchange, messageKey, true, new  RabbitMessage(msg));
    }
}

internal class OutboxLogger
{
    private readonly ILogger<Outbox> logger;

    public OutboxLogger(ILogger<Outbox> logger)
    {
        this.logger = logger;
    }

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
        logger.LogError(e, "Failed to push message from outbox");
    }
}

internal class RabbitMessage : IMessage
{
    private readonly Message outboxMessage;

    public RabbitMessage(Message outboxMessage)
    {
        this.outboxMessage = outboxMessage;
    }

    public object GetBody()
    {
        return outboxMessage.RecreateMessage();
    }

    public MessageProperties Properties => new MessageProperties();
    
    public Type MessageType => System.Type.GetType(outboxMessage.Type);
}