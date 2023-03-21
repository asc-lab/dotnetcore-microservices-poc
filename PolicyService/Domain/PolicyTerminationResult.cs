namespace PolicyService.Domain;

public class PolicyTerminationResult
{
    public PolicyTerminationResult(PolicyVersion terminalVersion, decimal amountToReturn)
    {
        TerminalVersion = terminalVersion;
        AmountToReturn = amountToReturn;
    }

    public PolicyVersion TerminalVersion { get; }
    public decimal AmountToReturn { get; }
}