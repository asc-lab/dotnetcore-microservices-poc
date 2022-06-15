using Prometheus;
using static System.Decimal;

namespace PolicyService.Metrics
{
    public static class MetricsRegistry
    {
        private static readonly Counter OffersCounter = Prometheus.Metrics.CreateCounter
        (
            "policy_offers_count",
            "Number of offers created",
            new[] { "ProductCode" }
        );

        private static readonly Counter PolicyCounter = Prometheus.Metrics.CreateCounter
        (
            "policy_policies_count",
            "Number of policies created",
            new[] { "ProductCode" }
        );
        
        private static readonly Summary PolicySalesSummary = Prometheus.Metrics.CreateSummary
        (
            "policy_policy_total_pln",
            "Summary of policy sold (PLN)",
            new []{ "ProductCode"}
        );

        public static void RegisterOfferCreation(string productCode)
        {
            OffersCounter.WithLabels(productCode).Inc();
        }

        public static void RegisterPolicyCreation(string productCode, decimal price)
        {
            PolicyCounter.WithLabels(productCode).Inc();
            PolicySalesSummary.WithLabels(productCode).Observe(ToDouble(price));
        }
}
}