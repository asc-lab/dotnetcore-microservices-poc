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
    public class PricingTravelTest
    {
        public Scenario CreateScenario()
        {
            var step = Step.Create("calculate_price", async context =>
            {
                using var httpClient = new HttpClient();

                var randomizer = new TravelDataRandomizer();
                
                var request = new CalculatePriceCommand
                {
                    ProductCode = "TRI",
                    PolicyFrom = DateTimeOffset.Now.AddDays(5),
                    PolicyTo = DateTimeOffset.Now.AddDays(5).AddDays(randomizer.TripLength()),
                    SelectedCovers = new List<string> { "C1", "C2"},
                    Answers = new List<QuestionAnswer>
                    {
                        new NumericQuestionAnswer { QuestionCode = "NUM_OF_ADULTS", Answer = randomizer.NumberOfAdults()},
                        new NumericQuestionAnswer { QuestionCode = "NUM_OF_CHILDREN", Answer = randomizer.NumberOfChildren()},
                        new ChoiceQuestionAnswer { QuestionCode = "DESTINATION", Answer = randomizer.Destination()}
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
                    return Response.Fail();
                }
                
            });

            return ScenarioBuilder.CreateScenario("price_travel", step);
        }
    }

    public class TravelDataRandomizer
    {
        private readonly Random random = new Random();

        public int TripLength() => random.Next(100) switch
        {
            <=50 => 7,
            >50 and < 80 => 14,
            _ => 21
        };

        public decimal NumberOfAdults() => random.Next(100) switch
        {
            <=10 => 1,
            >10 and <70 => 2,
            >70 and <90 => 3,
            _ => 4
        };
        
        public decimal NumberOfChildren() => random.Next(100) switch
        {
            <=10 => 0,
            >10 and <70 => 2,
            >70 and <90 => 1,
            _ => 4
        };
        
        public string Destination() => random.Next(100) switch
        {
            <=50 => "WORLD",
            >50 and < 80 => "EUR",
            _ => "PL"
        };
    }
}