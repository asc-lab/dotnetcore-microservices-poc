namespace DashboardService.Api.Queries.Dtos
{
    public class SalesDto
    {
        public long PoliciesCount { get; set; }
        public decimal PremiumAmount { get; set; }

        public SalesDto()
        {
        }

        public SalesDto(long policiesCount, decimal premiumAmount)
        {
            PoliciesCount = policiesCount;
            PremiumAmount = premiumAmount;
        }
    }
}