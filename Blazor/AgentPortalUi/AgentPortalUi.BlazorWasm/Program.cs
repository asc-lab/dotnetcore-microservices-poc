using AgentPortalUi.BlazorWasm.Contracts;
using AgentPortalUi.BlazorWasm.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AgentPortalUi.BlazorWasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<UserService, UserService>();
            builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
            builder.Services.AddScoped<IProductsService, ProductsService>();


            var host = builder.Build();

            var authenticationService = host.Services.GetRequiredService<UserService>();
            await authenticationService.Initialize();
            
            await host.RunAsync();
        }
    }
}
