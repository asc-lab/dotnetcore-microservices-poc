namespace ProductService.Api.Commands.Dtos;

public class CoverDto
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool Optional { get; set; }
    public decimal? SumInsured { get; set; }
}