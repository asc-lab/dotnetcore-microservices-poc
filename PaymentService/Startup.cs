using System;
using System.Collections.Generic;
using GlobalExceptionHandler.WebApi;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentService.Configuration;
using PaymentService.DataAccess.Marten;
using PaymentService.Domain;
using PaymentService.Infrastructure;
using PaymentService.Init;
using PaymentService.Jobs;
using PaymentService.Messaging.RabbitMq;
using PolicyService.Api.Events;

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
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt =>
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseGlobalExceptionHandler(cfg => cfg.MapExceptions());
            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseInitializer();
            app.UseRabbitListeners(new List<Type> { typeof(PolicyCreated), typeof(PolicyTerminated) });
            app.UseBackgroundJobs();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
