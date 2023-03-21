using System.Collections.Generic;
using System.Linq;
using ProductService.Api.Queries.Dtos;
using ProductService.Domain;
using ProductService.Queries;
using ProductService.Test.TestData;
using Xunit;

namespace ProductService.Test.Handlers;

public class ProductMapperTest
{
    [Fact]
    public void ToCoverDtoList_ReturnsCorrectTypeAndValuesAndNumberOfElements()
    {
        var cover = new Cover("C1", "Crops", "", false, 200000);
        IList<Cover> covers = new List<Cover>
        {
            cover,
            new("C2", "Flood", "", false, 100000),
            new("C3", "Fire", "", false, 50000),
            new("C4", "Equipment", "", true, 300000)
        };

        var result = ProductMapper.ToCoverDtoList(covers);
        var resultCover = result?.First(c => c.Code == cover.Code);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(covers.Count, result.Count);
        Assert.NotNull(resultCover);
        Assert.Equal(cover.Name, resultCover.Name);
        Assert.Equal(cover.Description, resultCover.Description);
        Assert.Equal(cover.Optional, resultCover.Optional);
        Assert.Equal(cover.SumInsured, resultCover.SumInsured);
    }

    [Fact]
    public void ToQuestionDtoList_ReturnsCorrectTypeAndNumberOfElements()
    {
        var choiceQuestion = TestQuestionFactory.ChoiceQuestion();
        var numricQuestion = TestQuestionFactory.NumericQuestion();
        var dateQuestion = TestQuestionFactory.DateQuestion();

        var questions = new List<Question>
        {
            choiceQuestion,
            numricQuestion,
            dateQuestion
        };

        var result = ProductMapper.ToQuestionDtoList(questions);

        var resultChoiceQuestion = result?.First(r => r.QuestionCode == choiceQuestion.Code);
        var resultNumericQuestion = result?.First(r => r.QuestionCode == numricQuestion.Code);
        var resultDateQuestion = result?.First(r => r.QuestionCode == dateQuestion.Code);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(questions.Count, result.Count);
        Assert.NotNull(resultChoiceQuestion);
        Assert.IsType<ChoiceQuestionDto>(resultChoiceQuestion);
        Assert.NotNull(resultNumericQuestion);
        Assert.IsType<NumericQuestionDto>(resultNumericQuestion);
        Assert.NotNull(resultDateQuestion);
        Assert.IsType<DateQuestionDto>(resultDateQuestion);
    }

    [Fact]
    public void ToQuestionDtoList_ReturnsCorrectValue()
    {
        var choiceQuestion = TestQuestionFactory.ChoiceQuestion();

        var questions = new List<Question>
        {
            choiceQuestion
        };

        var result = ProductMapper.ToQuestionDtoList(questions);
        var resultQuestion = result?.First(r => r.QuestionCode == choiceQuestion.Code) as ChoiceQuestionDto;

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.NotNull(resultQuestion);


        Assert.Equal(choiceQuestion.Index, resultQuestion.Index);
        Assert.Equal(choiceQuestion.Text, resultQuestion.Text);
        Assert.NotNull(resultQuestion.Choices);
        Assert.Equal(choiceQuestion.Choices.Count, resultQuestion.Choices.Count);
    }
}