using PricingService.Api.Commands.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.Domain
{
    public abstract class Answer
    {
        public string QuestionCode { get; protected set; }

        public abstract object GetAnswerValue();

        public static Answer Create(PolicyService.Api.Commands.Dtos.QuestionType questionType, string questionCode, object answerValue)
        {
            switch (questionType)
            {
                case PolicyService.Api.Commands.Dtos.QuestionType.Text:
                    return new TextAnswer(questionCode, (string)answerValue);
                case PolicyService.Api.Commands.Dtos.QuestionType.Choice:
                    return new ChoiceAnswer(questionCode, (string)answerValue);
                case PolicyService.Api.Commands.Dtos.QuestionType.Numeric:
                    return new NumericAnswer(questionCode, (decimal)answerValue);
                default:
                    throw new ArgumentException();
            }
        }
    }

    public abstract class Answer<T> : Answer
    {
        public T AnswerValue { get; protected set; }

        public override object GetAnswerValue() => AnswerValue;
    }

    public class TextAnswer : Answer<string>
    {
        public TextAnswer() { } //NH required

        public TextAnswer(string questionCode, string answer)
        {
            this.QuestionCode = questionCode;
            this.AnswerValue = answer;
        }
    }

    public class NumericAnswer : Answer<decimal>
    {
        public NumericAnswer() { } //NH required

        public NumericAnswer(string questionCode, decimal answer)
        {
            this.QuestionCode = questionCode;
            this.AnswerValue = answer;
        }
    }

    public class ChoiceAnswer : Answer<string>
    {
        public ChoiceAnswer() { } //NH required

        public ChoiceAnswer(string questionCode, string answer)
        {
            this.QuestionCode = questionCode;
            this.AnswerValue = answer;
        }
    }
}
