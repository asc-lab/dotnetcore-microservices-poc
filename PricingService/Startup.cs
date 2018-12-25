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
using PricingService.Configuration;
using PricingService.DataAccess.Marten;
using PricingService.Init;
using GlobalExceptionHandler.WebApi;
using PricingService.Infrastructure;
using Steeltoe.Discovery.Client;

namespace PricingService
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
            services.AddDiscoveryClient(Configuration);
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt => 
                {
                    opt.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
                });
            
            services.AddMarten(Configuration.GetConnectionString("DefaultConnection"));
            services.AddPricingDemoInitializer();
            services.AddMediatR();
            services.AddLogingBehaviour();
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
            app.UseInitializer();oot:run
            app.UseDiscoveryClient();
        }
    }
}
