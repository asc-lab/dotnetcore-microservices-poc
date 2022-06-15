using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using PricingService.Api.Commands;
using PricingService.Api.Commands.Dto;

namespace PerformanceTest
{
    public class PricingFarmTest
    {
        public Scenario CreateScenario()
        {
            var step = Step.Create("calculate_price", async context =>
            {
                using var httpClient = new HttpClient();

                var randomizer = new FarmDataRandomizer();
                
                var request = new CalculatePriceCommand
                {
                    ProductCode = "FAI",
                    PolicyFrom = DateTimeOffset.Now.AddDays(5),
                    PolicyTo = DateTimeOffset.Now.AddDays(4).AddYears(1),
                    SelectedCovers = new List<string> { "C1", "C2","C3","C4"},
                    Answers = new List<QuestionAnswer>
                    {
                        new ChoiceQuestionAnswer { QuestionCode = "TYP", Answer = randomizer.CultivationType()},
                        new NumericQuestionAnswer { QuestionCode = "AREA", Answer = randomizer.Area()},
                        new NumericQuestionAnswer { QuestionCode = "NUM_OF_CLAIM", Answer = randomizer.NumOfClaims()}
                    }
                };

                try
                {
                    var response = await httpClient.PostAsync
                    (
                        "http://localhost:5000/api/pricing",
                        new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
                    );

                    return response.IsSuccessStatusCode ? Response.Ok(response.StatusCode) : Response.Fail();
                }
                catch
                {
                    return    Response.Fail();
                }
                
            });

            return ScenarioBuilder.CreateScenario("price_farm", step);
        }
    }

    public class FarmDataRandomizer
    {
        private readonly Random random = new Random();

        public int Area() => random.Next(100) switch
        {
            <= 50 => 10,
            > 50 and < 80 => 30,
            _ => 150
        };

        public string CultivationType() => random.Next(100) switch
        {
            < 75 => "ZB",
            _ => "KW"
        };

        public decimal NumOfClaims() => random.Next(100) switch
        {
            < 60 => 0,
            >= 60 and < 80 => 3,
            _ => 5
        };
    }
}