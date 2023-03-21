namespace DashboardService.Domain;

public class SalesResult
{
    public SalesResult(long policiesCount, decimal premiumAmount)
    {
        PoliciesCount = policiesCount;
        PremiumAmount = premiumAmount;
    }

    public long PoliciesCount { get; }
    public decimal PremiumAmount { get; }

    public override string ToString()
    {
        return $"count: {PoliciesCount} amount: {PremiumAmount}";
    }

    public static SalesResult NoSale()
    {
        return new SalesResult(0, 0M);
    }
}