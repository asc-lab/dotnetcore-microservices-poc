using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using NBomber.Contracts;
using NBomber.CSharp;
using Newtonsoft.Json;
using PolicyService.Api.Commands;
using PolicyService.Api.Commands.Dtos;

namespace PerformanceTest
{
    public class PolicyTravelTest
    {
        public Scenario CreateScenario()
        {
            var createOffer = Step.Create("create_offer", async context =>
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("AgentLogin","jimmy.solid");

                var randomizer = new TravelDataRandomizer();
                
                var request = new CreateOfferCommand
                {
                    ProductCode = "TRI",
                    PolicyFrom = DateTime.Now.AddDays(5),
                    PolicyTo = DateTime.Now.AddDays(5).AddDays(randomizer.TripLength()),
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
                        "http://localhost:5050/api/offer",
                        new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
                    );

                    if (response.IsSuccessStatusCode)
                    {

                        var offerCreationResult =
                            JsonConvert.DeserializeObject<CreateOfferResult>(await response.Content.ReadAsStringAsync());

                        return Response.Ok(offerCreationResult.OfferNumber);
                    }
                    
                    return Response.Fail();
                }
                catch
                {
                    return Response.Fail();
                }
                
            });
            
            var convertToPolicy = Step.Create("convert_offer", async context =>
            {
                var randomizer = new Random();

                if (randomizer.Next(100)>55)
                    return Response.Ok();
                
                using var httpClient = new HttpClient();
                var request = new CreatePolicyCommand
                {
                    OfferNumber = context.GetPreviousStepResponse<string>(),
                    PolicyHolder = new PersonDto
                    {
                        FirstName = "Jan",
                        LastName = "Nowak",
                        TaxId = "11111111116"
                    },
                    PolicyHolderAddress = new AddressDto
                    {
                        City = "Warsaw",
                        Country = "POLAND",
                        Street = "Ulica",
                        ZipCode = "01-001"
                    }
                };

                try
                {
                    var response = await httpClient.PostAsync
                    (
                        "http://localhost:5050/api/policy",
                        new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json")
                    );

                    return response.IsSuccessStatusCode ? Response.Ok(response.StatusCode) : Response.Fail();
                }
                catch
                {
                    return Response.Fail();
                }
                
            });

            return ScenarioBuilder.CreateScenario("create_policy_travel", createOffer, convertToPolicy);
        }
    }
}