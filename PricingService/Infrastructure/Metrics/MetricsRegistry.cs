using Prometheus;
using static System.Decimal;

namespace PricingService.Infrastructure.Metrics
{
    public static class MetricsRegistry
    {
        private static readonly Counter CalculationsCounter = Prometheus.Metrics.CreateCounter
        (
            "pricing_calculations_count",
            "Number of calculations",
            new []{ "ProductCode"}
        );

        private static readonly Histogram PolicyPriceHistogram = Prometheus.Metrics.CreateHistogram
        (
            "pricing_policy_quote_pln",
            "Histogram of policy prices (PLN)",
            new HistogramConfiguration
            {
                Buckets = Histogram.LinearBuckets(0,500,10),
                LabelNames = new []{ "ProductCode"}
            }
        );
        
        private static readonly Summary PolicyPriceSummary = Prometheus.Metrics.CreateSummary
        (
            "pricing_policy_total_pln",
            "Summary of policy prices (PLN)",
            new []{ "ProductCode"}
        );

        public static void ReportPriceCalculation(string productCode, decimal price)
        {
            CalculationsCounter.WithLabels(productCode).Inc();    
            PolicyPriceHistogram.WithLabels(productCode).Observe(ToDouble(price));
            PolicyPriceSummary.WithLabels(productCode).Observe(ToDouble(price));
        }
        
    }
}