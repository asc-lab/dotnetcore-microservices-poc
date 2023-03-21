using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alba;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Xunit;

namespace PricingService.IntegrationTest;

public class IntegrationTestsFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer pgSqlContainer = new PostgreSqlBuilder()
        .WithDatabase("lab_netmicro_pricing")
        .WithCleanUp(true)
        .Build();

    public IAlbaHost SystemUnderTest { get; private set; }

    public async Task InitializeAsync()
    {
        await pgSqlContainer.StartAsync();

        var hostBuilder = Program.CreateWebHostBuilder(Array.Empty<string>())
            .ConfigureServices((Action<HostBuilderContext, IServiceCollection>)SetupServices)
            .ConfigureAppConfiguration((ctx, configBuilder) =>
            {
                configBuilder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["ConnectionStrings:DefaultConnection"] = pgSqlContainer.GetConnectionString()
                });
            });

        SystemUnderTest = new AlbaHost(hostBuilder);
    }

    public async Task DisposeAsync()
    {
        await SystemUnderTest.DisposeAsync();
        await pgSqlContainer.DisposeAsync();
    }

    protected virtual void SetupServices(HostBuilderContext ctx, IServiceCollection services)
    {
    }

    protected Task SetupData()
    {
        return Task.CompletedTask;
    }
}