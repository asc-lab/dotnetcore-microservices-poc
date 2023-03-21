using System.Collections.Generic;
using ProductService.Api.Queries.Dtos;

namespace ProductService.Test.TestData;

internal static class TestProductDtoFactory
{
    internal static ProductDto Travel()
    {
        return new ProductDto
        {
            Code = "TRI",
            Name = "Safe Traveller",
            Description = "Travel insurance",
            Image = "/static/travel.jpg",
            MaxNumberOfInsured = 10,
            Covers = new List<CoverDto>
            {
                new()
                {
                    Code = "C2", Name = "Illness", Description = "", Optional = false, SumInsured = 5000
                }
            },
            Questions = new List<QuestionDto>
            {
                new ChoiceQuestionDto
                {
                    QuestionCode = "DESTINATION", Index = 1, Text = "Destination",
                    Choices = new List<ChoiceDto> { new() { Code = "EUR", Label = "Europe" } }
                }
            }
        };
    }
}