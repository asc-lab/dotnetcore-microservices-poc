using Newtonsoft.Json;
using PricingService.Api.Converters;

namespace PricingService.Api.Commands.Dto
{
    [JsonConverter(typeof(QuestionAnswerConverter))]
    public abstract class QuestionAnswer
    {
        public string QuestionCode { get; set; }
        public abstract QuestionType QuestionType {get; }
        public abstract object GetAnswer();
        
    }

    public enum QuestionType
    {
        Text,
        Number,
        Choice
    }

    public abstract class QuestionAnswer<T> : QuestionAnswer
    {
        public T Answer { get; set; }

        public override object GetAnswer() => Answer;
    }

    public class TextQuestionAnswer : QuestionAnswer<string>
    {
        public override QuestionType QuestionType => QuestionType.Text;
    }


    public class NumericQuestionAnswer : QuestionAnswer<decimal>
    {
        public override QuestionType QuestionType => QuestionType.Number;
    }

    public class ChoiceQuestionAnswer : QuestionAnswer<string>
    {
        public override QuestionType QuestionType => QuestionType.Choice;
    }
}
