using Newtonsoft.Json;
using ProductService.Api.Queries.Converters;
using System.Collections.Generic;

namespace ProductService.Api.Queries.Dtos
{
    [JsonConverter(typeof(QuestionDtoConverter))]
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
}
 