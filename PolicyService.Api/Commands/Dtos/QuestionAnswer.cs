using Newtonsoft.Json;
using PolicyService.Api.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyService.Api.Commands.Dtos
{
    [JsonConverter(typeof(QuestionAnswerConverter))]
    public abstract class QuestionAnswer
    {
        public string QuestionCode { get; set; }
        public abstract QuestionType QuestionType { get; }
        public abstract Object GetAnswer();

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
