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
using PolicySearchService.DataAccess.ElasticSearch;
using PolicySearchService.Messaging.RabbitMq;
using PolicyService.Api.Events;
using Steeltoe.Discovery.Client;
using Microsoft.OpenApi.Models;

namespace PolicySearchService
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMediatR();
            services.AddElasticSearch(Configuration.GetConnectionString("ElasticSearchConnection"));
            services.AddRabbitListeners();
            services.AddSwaggerGen(c =>
            {
                string appVer = "v1";
                c.SwaggerDoc($"{appVer}", new OpenApiInfo
                {
                    Title = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} API",
                    Version = $"{appVer}"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseRabbitListeners(new List<Type> { typeof(PolicyCreated) });
            app.UseDiscoveryClient();

            string appVer = "v1";
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{appVer}/swagger.json", $"{this.GetType().Name} API {appVer}");
            });
        }
    }
}
