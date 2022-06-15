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
    public class PricingHouseTest
    {
        public Scenario CreateScenario()
        {
            var step = Step.Create("calculate_price", async context =>
            {
                using var httpClient = new HttpClient();

                var randomizer = new HouseDataRandomizer();
                
                var request = new CalculatePriceCommand
                {
                    ProductCode = "HSI",
                    PolicyFrom = DateTimeOffset.Now.AddDays(5),
                    PolicyTo = DateTimeOffset.Now.AddDays(4).AddYears(1),
                    SelectedCovers = new List<string> { "C1", "C2","C3","C4"},
                    Answers = new List<QuestionAnswer>
                    {
                        new ChoiceQuestionAnswer { QuestionCode = "TYP", Answer = randomizer.AptType()},
                        
                        new NumericQuestionAnswer { QuestionCode = "AREA", Answer = randomizer.Area()},
                        new NumericQuestionAnswer { QuestionCode = "NUM_OF_CLAIM", Answer = randomizer.NumOfClaims()},
                        new ChoiceQuestionAnswer { QuestionCode = "FLOOD", Answer = randomizer.Flood()}
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

            return ScenarioBuilder.CreateScenario("price_house", step);
        }
    }

    public class HouseDataRandomizer
    {
        private readonly Random random = new Random();

        public int Area() => random.Next(100) switch
        {
            <= 50 => 7,
            > 50 and < 80 => 14,
            _ => 21
        };

        public string AptType() => random.Next(100) switch
        {
            < 80 => "APT",
            _ => "HOUSE"
        };

        public decimal NumOfClaims() => random.Next(100) switch
        {
            < 60 => 0,
            >= 60 and < 80 => 1,
            _ => 3
        };
        
        public string Flood() => random.Next(100) switch
        {
            < 80 => "NO",
            _ => "YES"
        };
    }
}