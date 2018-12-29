namespace PolicyService.Domain
{
    public class PolicyTerminationResult
    {
        public PolicyVersion TerminalVersion { get; private set; }
        public decimal AmountToReturn { get; private set; }

        public PolicyTerminationResult(PolicyVersion terminalVersion, decimal amountToReturn)
        {
            TerminalVersion = terminalVersion;
            AmountToReturn = amountToReturn;
        }
    }
}