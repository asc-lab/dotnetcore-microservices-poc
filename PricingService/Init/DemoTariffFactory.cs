using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Init
{
    using Domain;

    internal static class DemoTariffFactory
    {
        internal static Tariff Travel()
        {
            var travel = new Tariff("TRI");
            travel.BasePremiumRules.AddBasePriceRule("C1", null, "(NUM_OF_ADULTS) * (DESTINATION == \"EUR\" ? 26.00M : 34.00M)");
            travel.BasePremiumRules.AddBasePriceRule("C2", null, "(NUM_OF_ADULTS + NUM_OF_CHILDREN) * 26.00M");
            travel.BasePremiumRules.AddBasePriceRule("C3", null, "(NUM_OF_ADULTS + NUM_OF_CHILDREN) * 10.00M");

            return travel;
        }
    }
}
