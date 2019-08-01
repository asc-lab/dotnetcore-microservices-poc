using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Domain
{
    public class BasePremiumCalculationRuleList
    {
        private List<BasePremiumCalculationRule> rules;

        public BasePremiumCalculationRuleList(List<BasePremiumCalculationRule> rules)
        {
            this.rules = rules;
        }

        public void AddBasePriceRule(string coverCode, string applyIfFormula, string basePriceFormula)
        {
            rules.Add(new BasePremiumCalculationRule(coverCode, applyIfFormula, basePriceFormula));
        }

        public decimal CalculateBasePriceFor(Cover cover, Calculation calculation)
        {
            return rules
                .Where(r => r.Applies(cover,calculation))
                .Select(r => r.CalculateBasePrice(calculation))
                .FirstOrDefault();
        }
    }
}