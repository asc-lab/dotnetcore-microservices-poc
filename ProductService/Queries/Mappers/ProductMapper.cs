using ProductService.Api.Queries.Dtos;
using ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductService.Queries
{
    public static class ProductMapper
    {
        public static IList<CoverDto> ToCoverDtoList(IList<Cover> covers)
        {
            return covers?.Select(c => ToCoverDto(c)).ToList();
        }

        public static IList<QuestionDto> ToQuestionDtoList(IList<Question> questions)
        {
            return questions?.Select(q => ToQuestionDto(q)).ToList();
        }

        private static CoverDto ToCoverDto(Cover cover)
        {
            return new CoverDto
            {
                Code = cover.Code,
                Name = cover.Name,
                Description = cover.Description,
                Optional = cover.Optional,
                SumInsured = cover.SumInsured
            };
        }

        private static QuestionDto ToQuestionDto(Question question)
        {
            switch (question.GetType().Name)
            {
                case "NumericQuestion":
                    return new NumericQuestionDto { QuestionCode = question.Code, Index = question.Index, Text = question.Text };                    
                case "ChoiceQuestion":
                    return new ChoiceQuestionDto
                    {
                        QuestionCode = question.Code,
                        Index = question.Index,
                        Text = question.Text,
                        Choices = ((ChoiceQuestion)question).Choices?.Select(c => new ChoiceDto { Code = c.Code, Label = c.Label }).ToList()
                    };
                case "DateQuestion":
                    return new DateQuestionDto { QuestionCode = question.Code, Index = question.Index, Text = question.Text };
                    
                default:
                    throw new ArgumentOutOfRangeException(question.GetType().Name);
            }
        }
    }
}
