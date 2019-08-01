using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentService.Infrastructure;
using PaymentService.Init;
using GlobalExceptionHandler.WebApi;
using Hangfire;
using Hangfire.PostgreSql;
using PaymentService.Configuration;
using PaymentService.DataAccess.Marten;
using PaymentService.Domain;
using PaymentService.Jobs;
using PolicyService.Api.Events;
using PaymentService.Messaging.RabbitMq;

namespace PaymentService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                });

            services.AddMarten(Configuration.GetConnectionString("PgConnection"));
            services.AddPaymentDemoInitializer();
            services.AddMediatR();
            services.AddLogingBehaviour();
            services.AddSingleton<PolicyAccountNumberGenerator>();
            services.AddRabbitListeners();
            services.AddBackgroundJobs(Configuration.GetSection("BackgroundJobs").Get<BackgroundJobsConfig>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseGlobalExceptionHandler(cfg => cfg.MapExceptions());
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseInitializer();
            app.UseRabbitListeners(new List<Type> { typeof(PolicyCreated), typeof(PolicyTerminated) });
            app.UseBackgroundJobs();
        }
    }
}
