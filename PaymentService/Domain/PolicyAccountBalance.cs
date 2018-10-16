namespace PaymentService.Domain
{
    public class PolicyAccountBalance
    {
        public string PolicyAccountNumber { get; private set; }
        public string PolicyNumber { get; private set; }
        public decimal Balance { get; private set; }
    }
}
