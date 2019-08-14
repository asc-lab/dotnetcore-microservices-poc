using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PricingService.Domain
{
    public class Tariff
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        [JsonProperty]
        private List<BasePremiumCalculationRule> basePremiumRules;
        [JsonProperty]
        private List<DiscountMarkupRule> discountMarkupRules;

        [JsonIgnore]
        public BasePremiumCalculationRuleList BasePremiumRules => new BasePremiumCalculationRuleList(basePremiumRules);
        [JsonIgnore]
        public DiscountMarkupRuleList DiscountMarkupRules => new DiscountMarkupRuleList(discountMarkupRules);

        public Tariff(string code)
        {
            Id = Guid.NewGuid();
            Code = code;
            basePremiumRules = new List<BasePremiumCalculationRule>();
            discountMarkupRules = new List<DiscountMarkupRule>();
        }

        public Calculation CalculatePrice(Calculation calculation)
        {
            CalcBasePrices(calculation);
            ApplyDiscounts(calculation);
            UpdateTotals(calculation);
            return calculation;
        }

        

        private void CalcBasePrices(Calculation calculation)
        {
            foreach (var cover in calculation.Covers.Values)
            {
                cover.SetPrice(BasePremiumRules.CalculateBasePriceFor(cover,calculation));
            }
        }

        private void ApplyDiscounts(Calculation calculation)
        {
            DiscountMarkupRules.Apply(calculation);
        }

        private void UpdateTotals(Calculation calculation)
        {
            calculation.UpdateTotal();
        }
    }
}
