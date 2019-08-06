using System;
using PolicyService.Domain;
using PolicyService.Test.Domain;

namespace PolicyService.Test.TestData
{
    public class PolicyFactory
    {
        internal static Policy AlreadyTerminatedPolicy()
        {
            var offer = OfferFactory.NewOfferValidUntil(DateTime.Now.AddDays(5));

            var policy = offer.Buy(PolicyHolderFactory.Abc());

            policy.Terminate(DateTime.Now.AddDays(3));

            return policy;
        }
    }
}
