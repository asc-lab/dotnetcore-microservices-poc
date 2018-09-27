using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Domain
{
    public class Tariff
    {
        public string Code { get; private set; }
        private List<BasePremiumCalculationRule> basePremiumRules;
        private List<DiscountMarkupRule> discountMarkupRules;

        public BasePremiumCalculationRuleList BasePremiumRules => new BasePremiumCalculationRuleList(basePremiumRules);

        public DiscountMarkupRuleList DiscountMarkupRules => new DiscountMarkupRuleList(discountMarkupRules);

        public Tariff(string code)
        {
            Code = code;
            this.basePremiumRules = new List<BasePremiumCalculationRule>();
            this.discountMarkupRules = new List<DiscountMarkupRule>();
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
