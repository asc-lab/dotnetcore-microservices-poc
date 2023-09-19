using PolicyService.Api.Commands.Dtos;
using ProductService.Api.Queries.Dtos;

namespace BlazorClient.Components.Products;

public class OfferParametersModel
{
    public DateTime? PolicyFrom { get; set; }
    public DateTime? PolicyTo { get; set; }
    public List<QuestionAnswerModel> QuestionAnswers { get; } = new();

    public void InitQuestions(ProductDto product)
    {
        foreach (var question in product.Questions)
        {
            switch (question)
            {
                case ChoiceQuestionDto choiceQuestion:
                    QuestionAnswers.Add(new QuestionAnswerModel
                    {
                        Question = choiceQuestion.Text,
                        Choices = choiceQuestion.Choices,
                        Answer = new ChoiceQuestionAnswer
                        {
                            QuestionCode = choiceQuestion.QuestionCode
                        }
                    });
                    break;
                case NumericQuestionDto numericQuestion:
                    QuestionAnswers.Add(new QuestionAnswerModel
                    {
                        Question = numericQuestion.Text,
                        Answer = new NumericQuestionAnswer
                        {
                            QuestionCode = numericQuestion.QuestionCode
                        }
                    });
                    break;
            }
        }
    }
    
    public Task SetChoiceAnswer(string answerQuestionCode, string value)
    {
        var answer = QuestionAnswers.First(q => q.Answer.QuestionCode == answerQuestionCode);
        (answer.Answer as ChoiceQuestionAnswer).Answer = value;
        return Task.CompletedTask;
    }
    
    public Task SetNumericAnswer(string answerQuestionCode, decimal? value)
    {
        var answer = QuestionAnswers.First(q => q.Answer.QuestionCode == answerQuestionCode);
        (answer.Answer as NumericQuestionAnswer).Answer = value ?? 0;
        return Task.CompletedTask;
    }
}

public class QuestionAnswerModel
{
    public string Question { get; set; }
    public IList<ChoiceDto> Choices { get; set; }
    public QuestionAnswer Answer { get; set; }
    
}