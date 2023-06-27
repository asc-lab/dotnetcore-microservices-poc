using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DashboardService.Domain;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;

namespace DashboardService.Init;

public class SalesData
{
    private readonly ElasticsearchClient elasticClient;

    public SalesData(ElasticsearchClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public async Task SeedData()
    {
        var policyIndexExists = await elasticClient.Indices.ExistsAsync("policy_lab_stats");
        if (policyIndexExists.Exists) await elasticClient.Indices.DeleteAsync(new DeleteIndexRequest("policy_lab_stats"));

        var salesData = new Dictionary<string, (string Product, int Month, int Policies)[]>
        {
            { "jimmy.solid", JimmySalesData() },
            { "danny.solid", DannySalesData() },
            { "admin", AdminSalesData() }
        };

        foreach (var agentSalesData in salesData)
        foreach (var (product, month, policies) in agentSalesData.Value)
        {
            var startMonth = DateTime.Now.AddMonths(-1 * month);

            for (var index = 0; index < policies; index++)
            {
                var policy = new PolicyDocument
                (
                    Guid.NewGuid().ToString(),
                    new DateTime(startMonth.Year, startMonth.Month, 1),
                    new DateTime(startMonth.Year, startMonth.Month, 1).AddYears(1).AddDays(-1),
                    "Anonymous Mike",
                    product,
                    100M,
                    agentSalesData.Key
                );

                await elasticClient.IndexAsync
                (
                    policy,
                    i => i
                        .Index("policy_lab_stats")
                        .Id(policy.Number)
                        .Refresh(Refresh.True)
                );
            }
        }
    }

    private static (string Product, int Month, int Policies)[] JimmySalesData()
    {
        return new[]
        {
            (Product: "TRI", Month: 1, Policies: 10),
            (Product: "HSI", Month: 1, Policies: 9),
            (Product: "FAI", Month: 1, Policies: 8),
            (Product: "CAR", Month: 1, Policies: 7),

            (Product: "TRI", Month: 2, Policies: 6),
            (Product: "HSI", Month: 2, Policies: 5),
            (Product: "FAI", Month: 2, Policies: 6),
            (Product: "CAR", Month: 2, Policies: 5),

            (Product: "TRI", Month: 3, Policies: 7),
            (Product: "HSI", Month: 3, Policies: 5),
            (Product: "FAI", Month: 3, Policies: 8),
            (Product: "CAR", Month: 3, Policies: 8),

            (Product: "TRI", Month: 4, Policies: 10),
            (Product: "HSI", Month: 4, Policies: 11),
            (Product: "FAI", Month: 4, Policies: 12),
            (Product: "CAR", Month: 4, Policies: 14),

            (Product: "TRI", Month: 5, Policies: 14),
            (Product: "HSI", Month: 5, Policies: 14),
            (Product: "FAI", Month: 5, Policies: 10),
            (Product: "CAR", Month: 5, Policies: 10),

            (Product: "TRI", Month: 6, Policies: 14),
            (Product: "HSI", Month: 6, Policies: 16),
            (Product: "FAI", Month: 6, Policies: 12),
            (Product: "CAR", Month: 6, Policies: 12),

            (Product: "TRI", Month: 7, Policies: 10),
            (Product: "HSI", Month: 7, Policies: 10),
            (Product: "FAI", Month: 7, Policies: 8),
            (Product: "CAR", Month: 7, Policies: 9),

            (Product: "TRI", Month: 8, Policies: 10),
            (Product: "HSI", Month: 8, Policies: 10),
            (Product: "FAI", Month: 8, Policies: 8),
            (Product: "CAR", Month: 8, Policies: 9),

            (Product: "TRI", Month: 9, Policies: 12),
            (Product: "HSI", Month: 9, Policies: 12),
            (Product: "FAI", Month: 9, Policies: 12),
            (Product: "CAR", Month: 9, Policies: 11),

            (Product: "TRI", Month: 10, Policies: 14),
            (Product: "HSI", Month: 10, Policies: 10),
            (Product: "FAI", Month: 10, Policies: 10),
            (Product: "CAR", Month: 10, Policies: 8),

            (Product: "TRI", Month: 11, Policies: 10),
            (Product: "HSI", Month: 11, Policies: 8),
            (Product: "FAI", Month: 11, Policies: 8),
            (Product: "CAR", Month: 11, Policies: 6),

            (Product: "TRI", Month: 12, Policies: 9),
            (Product: "HSI", Month: 12, Policies: 4),
            (Product: "FAI", Month: 12, Policies: 6),
            (Product: "CAR", Month: 12, Policies: 10)
        };
    }

    private static (string Product, int Month, int Policies)[] DannySalesData()
    {
        return new[]
        {
            (Product: "TRI", Month: 1, Policies: 8),
            (Product: "HSI", Month: 1, Policies: 7),
            (Product: "FAI", Month: 1, Policies: 6),
            (Product: "CAR", Month: 1, Policies: 5),

            (Product: "TRI", Month: 2, Policies: 4),
            (Product: "HSI", Month: 2, Policies: 3),
            (Product: "FAI", Month: 2, Policies: 3),
            (Product: "CAR", Month: 2, Policies: 8),

            (Product: "TRI", Month: 3, Policies: 5),
            (Product: "HSI", Month: 3, Policies: 5),
            (Product: "FAI", Month: 3, Policies: 3),
            (Product: "CAR", Month: 3, Policies: 12),

            (Product: "TRI", Month: 4, Policies: 3),
            (Product: "HSI", Month: 4, Policies: 3),
            (Product: "FAI", Month: 4, Policies: 3),
            (Product: "CAR", Month: 4, Policies: 3),

            (Product: "TRI", Month: 5, Policies: 10),
            (Product: "HSI", Month: 5, Policies: 7),
            (Product: "FAI", Month: 5, Policies: 7),
            (Product: "CAR", Month: 5, Policies: 7),

            (Product: "TRI", Month: 6, Policies: 12),
            (Product: "HSI", Month: 6, Policies: 10),
            (Product: "FAI", Month: 6, Policies: 12),
            (Product: "CAR", Month: 6, Policies: 12),

            (Product: "TRI", Month: 7, Policies: 5),
            (Product: "HSI", Month: 7, Policies: 10),
            (Product: "FAI", Month: 7, Policies: 5),
            (Product: "CAR", Month: 7, Policies: 4),

            (Product: "TRI", Month: 8, Policies: 6),
            (Product: "HSI", Month: 8, Policies: 11),
            (Product: "FAI", Month: 8, Policies: 6),
            (Product: "CAR", Month: 8, Policies: 8),

            (Product: "TRI", Month: 9, Policies: 8),
            (Product: "HSI", Month: 9, Policies: 15),
            (Product: "FAI", Month: 9, Policies: 2),
            (Product: "CAR", Month: 9, Policies: 2),

            (Product: "TRI", Month: 10, Policies: 8),
            (Product: "HSI", Month: 10, Policies: 10),
            (Product: "FAI", Month: 10, Policies: 2),
            (Product: "CAR", Month: 10, Policies: 4),

            (Product: "TRI", Month: 11, Policies: 10),
            (Product: "HSI", Month: 11, Policies: 12),
            (Product: "FAI", Month: 11, Policies: 4),
            (Product: "CAR", Month: 11, Policies: 4),

            (Product: "TRI", Month: 12, Policies: 8),
            (Product: "HSI", Month: 12, Policies: 2),
            (Product: "FAI", Month: 12, Policies: 2),
            (Product: "CAR", Month: 12, Policies: 5)
        };
    }

    private static (string Product, int Month, int Policies)[] AdminSalesData()
    {
        return new[]
        {
            (Product: "TRI", Month: 1, Policies: 7),
            (Product: "HSI", Month: 1, Policies: 6),
            (Product: "FAI", Month: 1, Policies: 5),
            (Product: "CAR", Month: 1, Policies: 4),

            (Product: "TRI", Month: 2, Policies: 3),
            (Product: "HSI", Month: 2, Policies: 2),
            (Product: "FAI", Month: 2, Policies: 2),
            (Product: "CAR", Month: 2, Policies: 7),

            (Product: "TRI", Month: 3, Policies: 4),
            (Product: "HSI", Month: 3, Policies: 4),
            (Product: "FAI", Month: 3, Policies: 2),
            (Product: "CAR", Month: 3, Policies: 5),

            (Product: "TRI", Month: 4, Policies: 2),
            (Product: "HSI", Month: 4, Policies: 2),
            (Product: "FAI", Month: 4, Policies: 2),
            (Product: "CAR", Month: 4, Policies: 2),

            (Product: "TRI", Month: 5, Policies: 1),
            (Product: "HSI", Month: 5, Policies: 5),
            (Product: "FAI", Month: 5, Policies: 5),
            (Product: "CAR", Month: 5, Policies: 5),

            (Product: "TRI", Month: 6, Policies: 5),
            (Product: "HSI", Month: 6, Policies: 3),
            (Product: "FAI", Month: 6, Policies: 3),
            (Product: "CAR", Month: 6, Policies: 3),

            (Product: "TRI", Month: 7, Policies: 4),
            (Product: "HSI", Month: 7, Policies: 4),
            (Product: "FAI", Month: 7, Policies: 4),
            (Product: "CAR", Month: 7, Policies: 4),

            (Product: "TRI", Month: 8, Policies: 6),
            (Product: "HSI", Month: 8, Policies: 6),
            (Product: "FAI", Month: 8, Policies: 6),
            (Product: "CAR", Month: 8, Policies: 6),

            (Product: "TRI", Month: 9, Policies: 2),
            (Product: "HSI", Month: 9, Policies: 4),
            (Product: "FAI", Month: 9, Policies: 2),
            (Product: "CAR", Month: 9, Policies: 2),

            (Product: "TRI", Month: 10, Policies: 4),
            (Product: "HSI", Month: 10, Policies: 5),
            (Product: "FAI", Month: 10, Policies: 1),
            (Product: "CAR", Month: 10, Policies: 1),

            (Product: "TRI", Month: 11, Policies: 1),
            (Product: "HSI", Month: 11, Policies: 1),
            (Product: "FAI", Month: 11, Policies: 4),
            (Product: "CAR", Month: 11, Policies: 4),

            (Product: "TRI", Month: 12, Policies: 5),
            (Product: "HSI", Month: 12, Policies: 2),
            (Product: "FAI", Month: 12, Policies: 2),
            (Product: "CAR", Month: 12, Policies: 2)
        };
    }
}