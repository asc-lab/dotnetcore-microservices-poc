using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Testing;
using PricingService.Api.Commands;
using PricingService.Api.Commands.Dto;
using Xunit;
using static Xunit.Assert;

namespace PricingService.IntegrationTest
{
    public class PricingControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public PricingControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void PriceForTravelPolicyIsCorrect()
        {
            var client = factory.CreateClient();

            var response = await client.DoPostAsync<CalculatePriceResult>("/api/Pricing", new CalculatePriceCommand
            {
                ProductCode = "TRI",
                PolicyFrom = DateTimeOffset.Now.AddDays(5),
                PolicyTo = DateTimeOffset.Now.AddDays(10),
                SelectedCovers = new List<string> {  "C1" , "C2", "C3"},
                Answers = new List<QuestionAnswer>
                {
                    new NumericQuestionAnswer { QuestionCode = "NUM_OF_ADULTS", Answer = 1M},
                    new NumericQuestionAnswer { QuestionCode = "NUM_OF_CHILDREN", Answer = 1M},
                    new TextQuestionAnswer { QuestionCode = "DESTINATION", Answer = "EUR"}
                }
            });

            True(response.Success);
            Equal(98M, response.Data.TotalPrice);
        }

        [Fact]
        public async void CommandIsProperlyValidated()
        {
            var client = factory.CreateClient();
            var response = await client.DoPostAsync<CalculatePriceResult>("/api/Pricing", new CalculatePriceCommand { });

            False(response.Success);
            Equal("400", response.ErrorCode);
        }
    }
}
