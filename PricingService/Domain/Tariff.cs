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

        public Tariff(string code)
        {
            Code = code;
            this.basePremiumRules = new List<BasePremiumCalculationRule>();
        }

        public Calculation CalculatePrice(Calculation calculation)
        {
            CalcBasePrices(calculation);
            calculation.UpdateTotal();
            return calculation;
        }

        public BasePremiumCalculationRuleList BasePremiumRules => new BasePremiumCalculationRuleList(basePremiumRules);

        private void CalcBasePrices(Calculation calculation)
        {
            foreach (var cover in calculation.Covers.Values)
            {
                cover.SetPrice(BasePremiumRules.CalculateBasePriceFor(cover,calculation));
            }
        }
    }
}
