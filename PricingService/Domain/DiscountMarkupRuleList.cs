using System.Collections.Generic;
using System.Linq;
using PricingService.Extensions;

namespace PricingService.Domain
{
    public class DiscountMarkupRuleList
    {
        private readonly List<DiscountMarkupRule> rules;

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
