using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PolicyService.Messaging.RabbitMq.Outbox
{
    public class OutboxSendingService : IHostedService
    {
        private readonly Outbox outbox;
        private Timer timer;
        private static object locker = new object();

        public OutboxSendingService(Outbox outbox)
        {
            this.outbox = outbox;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer
            (
                PushMessages,
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(1)
            );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        
        
        private async void PushMessages(object state)
        {
            var hasLock = false;

            try
            {
                Monitor.TryEnter(locker, ref hasLock);

                if (!hasLock)
                {
                    return;
                }
                
                await outbox.PushPendingMessages();

            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(locker);
                }
            }
        }
    }
}