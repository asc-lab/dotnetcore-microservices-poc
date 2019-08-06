using System.Collections.Generic;

namespace ProductService.Api.Queries.Dtos
{
    public class ProductDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public IList<CoverDto> Covers { get; set; }        
        public IList<QuestionDto> Questions { get; set; }
        public int MaxNumberOfInsured { get; set; }
    }
}
