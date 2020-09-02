namespace DashboardService.Domain
{
    public class SalesResult
    {
        public long PoliciesCount { get; }
        public decimal PremiumAmount { get; }

        public SalesResult(long policiesCount, decimal premiumAmount)
        {
            PoliciesCount = policiesCount;
            PremiumAmount = premiumAmount;
        }

        public override string ToString()
        {
            return $"count: {PoliciesCount} amount: {PremiumAmount}";
        }

        public static SalesResult NoSale()
        {
            return new SalesResult(0,0M);
        }
    }
}