using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.DataAccess.EF;
using ProductService.Init;
using Steeltoe.Discovery.Client;
using Microsoft.OpenApi.Models;

namespace ProductService
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
            services.AddEFConfiguration(Configuration);
            services.AddMvc()
                .AddJsonOptions(JsonOptions)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMediatR();
            services.AddProductDemoInitializer();
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
            app.UseInitializer();
            app.UseDiscoveryClient();

            string appVer = "v1";
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{appVer}/swagger.json", $"{this.GetType().Name} API {appVer}");
            });
        }
        
        private void JsonOptions(MvcJsonOptions options)
        {
            options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
        }
    }
}
