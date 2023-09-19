using BlazorClient.Components.Auth;
using BlazorClient.Components.Dashboard;
using BlazorClient.Components.PolicySearch;
using BlazorClient.Components.Products;
using Blazored.SessionStorage;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorClient;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices( IServiceCollection services )
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();

        services.AddServerSideBlazor().AddHubOptions( ( o ) =>
        {
            o.MaximumReceiveMessageSize = 1024 * 1024 * 100;
        } );

        AddBlazorise( services );

        services.AddBlazoredSessionStorage();
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

        services.AddScoped<PolicySearchClient>();
        services.AddScoped<ProductsClient>();
        services.AddScoped<PolicyClient>();
        services.AddScoped<AuthClient>();
        services.AddScoped<DashboardClient>();
        
        services.AddHttpClient();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure( IApplicationBuilder app, IWebHostEnvironment env )
    {
        if ( env.IsDevelopment() )
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // this is required to be here or otherwise the messages between server and client will be too large and
        // the connection will be lost.
        //app.UseSignalR( route => route.MapHub<ComponentHub>( ComponentHub.DefaultPath, o =>
        //{
        //    o.ApplicationMaxBufferSize = 1024 * 1024 * 100; // larger size
        //    o.TransportMaxBufferSize = 1024 * 1024 * 100; // larger size
        //} ) );

        app.UseEndpoints( endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage( "/_Host" );
        } );
    }

    public void AddBlazorise( IServiceCollection services )
    {
        services
            .AddBlazorise();
        services
            .AddBootstrap5Providers()
    .AddFontAwesomeIcons();
    }
}
