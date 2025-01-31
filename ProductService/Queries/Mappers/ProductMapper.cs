using ProductService.Api.Queries.Dtos;
using ProductService.Domain;

namespace ProductService.Queries;

public static class ProductMapper
{
    public static IList<CoverDto> ToCoverDtoList(IList<Cover> covers)
    {
        return covers?.Select(ToCoverDto).ToList();
    }

    public static IList<QuestionDto> ToQuestionDtoList(IList<Question> questions)
    {
        return questions?.Select(ToQuestionDto).ToList();
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
        return question.GetType().Name switch
        {
            "NumericQuestion" => new NumericQuestionDto
            {
                QuestionCode = question.Code, Index = question.Index, Text = question.Text
            },
            "ChoiceQuestion" => new ChoiceQuestionDto
            {
                QuestionCode = question.Code,
                Index = question.Index,
                Text = question.Text,
                Choices = ((ChoiceQuestion)question).Choices
                    ?.Select(c => new ChoiceDto { Code = c.Code, Label = c.Label })
                    .ToList()
            },
            "DateQuestion" => new DateQuestionDto
            {
                QuestionCode = question.Code, Index = question.Index, Text = question.Text
            },
            _ => throw new ArgumentOutOfRangeException(question.GetType().Name)
        };
    }
}