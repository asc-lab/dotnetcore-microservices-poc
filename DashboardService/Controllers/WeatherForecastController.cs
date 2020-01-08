using System;
using System.Collections.Generic;
using DashboardService.DataAccess.Elastic;
using DashboardService.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DashboardService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var r = new ElasticPolicyRepository();
            
            r.Save
            (
                new PolicyDocument
                    (
                    "POL0005",
                    new DateTime(2019,2,1),
                    new DateTime(2019,12,31),
                    "Jan Kowalski",
                    "BDA",
                    2000M,
                    "joe doe"
                    )
            );

            /*var x = r.GetAgentSales
            (
                new AgentSalesQuery
                (
                    "admin admin",
                    "BDA",
                    DateTime.MinValue, 
                    DateTime.MaxValue
                )
            );*/

            //var xx = r.FindByNumber("POL0001");

            var yy = r.GetAgentSales
            (
                new AgentSalesQuery(
                    null,
                    "BDA",
                    DateTime.MinValue,
                    DateTime.MaxValue
                )
            );

            var zz = r.GetTotalSales
            (
                new TotalSalesQuery
                (
                    null,
                    DateTime.MinValue,
                    DateTime.MaxValue
                )
            );

            var vv = r.GetSalesTrend
            (
                new SalesTrendsQuery
                (
                    null,
                    new DateTime(2019,1,1),
                    new DateTime(2019,12,31),
                    TimeAggregationUnit.Month
                )
            );
            
            return new List<string> {"ok"};
        }
    }
}