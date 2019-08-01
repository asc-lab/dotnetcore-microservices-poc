using PricingService.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Domain
{
    public class DiscountMarkupRuleList
    {
        private List<DiscountMarkupRule> rules;

        public DiscountMarkupRuleList(List<DiscountMarkupRule> rules)
        {
            this.rules = rules;
        }

        public void AddPercentMarkup(string applyIfFormula, decimal markup)
        {
            rules.Add(new PercentMarkupRule(applyIfFormula, markup));
        }

        public void Apply(Calculation calculation)
        {
            rules
                .Where(r => r.Applies(calculation))
                .ForEach(r => r.Apply(calculation));
        }
    }
}
