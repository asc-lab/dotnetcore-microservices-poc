using System.Collections.Generic;

namespace ProductService.Api.Commands.Dtos;

public class ProductDraftDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public IList<CoverDto> Covers { get; set; }
    public IList<QuestionDto> Questions { get; set; }
    public int MaxNumberOfInsured { get; set; }

    public string Icon { get; set; }
}