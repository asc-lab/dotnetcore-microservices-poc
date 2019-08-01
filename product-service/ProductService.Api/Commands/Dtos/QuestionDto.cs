using System.Collections.Generic;

namespace ProductService.Api.Commands.Dtos
{
    public abstract class QuestionDto
    {
        public string QuestionCode { get; set; }
        public int Index { get; set; }
        public string Text { get; set; }
        public abstract QuestionType QuestionType { get; }
    }
    
    public enum QuestionType
    {
        Choice,
        Date,
        Numeric
    }
    
    public class ChoiceQuestionDto : QuestionDto
    {        
        public IList<ChoiceDto> Choices { get; set; }
                
        public override QuestionType QuestionType => QuestionType.Choice;
    }

    public class DateQuestionDto : QuestionDto
    {
        public override QuestionType QuestionType => QuestionType.Date;
    }

    public class NumericQuestionDto : QuestionDto
    {
        public override QuestionType QuestionType => QuestionType.Numeric;
    }
    
    public class ChoiceDto
    {
        public string Code { get; set; }
        public string Label { get; set; }
    }
}