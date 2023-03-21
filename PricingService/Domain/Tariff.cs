using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PricingService.Domain;

public class Tariff
{
    [JsonProperty] private List<BasePremiumCalculationRule> basePremiumRules;

    [JsonProperty] private List<DiscountMarkupRule> discountMarkupRules;

    public Tariff(string code)
    {
        Id = Guid.NewGuid();
        Code = code;
        basePremiumRules = new List<BasePremiumCalculationRule>();
        discountMarkupRules = new List<DiscountMarkupRule>();
    }

    public Guid Id { get; }
    public string Code { get; }

    [JsonIgnore] public BasePremiumCalculationRuleList BasePremiumRules => new(basePremiumRules);

    [JsonIgnore] public DiscountMarkupRuleList DiscountMarkupRules => new(discountMarkupRules);

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
            cover.SetPrice(BasePremiumRules.CalculateBasePriceFor(cover, calculation));
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