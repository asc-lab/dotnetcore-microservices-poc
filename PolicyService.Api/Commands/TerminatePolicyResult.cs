namespace PolicyService.Api.Commands;

public class TerminatePolicyResult
{
    public string PolicyNumber { get; set; }
    public decimal MoneyToReturn { get; set; }
}