using System;
using PolicyService.Api.Commands.Dtos;

namespace PolicyService.Domain;

public abstract class Answer
{
    public string QuestionCode { get; protected set; }

    public abstract object GetAnswerValue();

    public static Answer Create(QuestionType questionType, string questionCode, object answerValue)
    {
        switch (questionType)
        {
            case QuestionType.Text:
                return new TextAnswer(questionCode, (string)answerValue);
            case QuestionType.Choice:
                return new ChoiceAnswer(questionCode, (string)answerValue);
            case QuestionType.Numeric:
                return new NumericAnswer(questionCode, (decimal)answerValue);
            default:
                throw new ArgumentException();
        }
    }
}

public abstract class Answer<T> : Answer
{
    public T AnswerValue { get; protected set; }

    public override object GetAnswerValue()
    {
        return AnswerValue;
    }
}

public class TextAnswer : Answer<string>
{
    public TextAnswer()
    {
    } //NH required

    public TextAnswer(string questionCode, string answer)
    {
        QuestionCode = questionCode;
        AnswerValue = answer;
    }
}

public class NumericAnswer : Answer<decimal>
{
    public NumericAnswer()
    {
    } //NH required

    public NumericAnswer(string questionCode, decimal answer)
    {
        QuestionCode = questionCode;
        AnswerValue = answer;
    }
}

public class ChoiceAnswer : Answer<string>
{
    public ChoiceAnswer()
    {
    } //NH required

    public ChoiceAnswer(string questionCode, string answer)
    {
        QuestionCode = questionCode;
        AnswerValue = answer;
    }
}