using Azure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure
{
    public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
                where TRequest : MediatR.IRequest<TResponse>
        {
        private readonly ILogger<TRequest> logger;

        public LoggingBehaviour(ILogger<TRequest> logger)
        {
            this.logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            logger.LogInformation("Handling {@Command}", typeof(TRequest));
            return next();
        }

                public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
                {
                        throw new System.NotImplementedException();
                }
        }

    public static class LogingBehaviourInstaller
    {
        public static IServiceCollection AddLogingBehaviour(this IServiceCollection services)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            return services;
        }
    }
}
