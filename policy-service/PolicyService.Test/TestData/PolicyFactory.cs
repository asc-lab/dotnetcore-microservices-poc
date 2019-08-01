using PolicyService.Domain;
using PolicyService.Test.Domain;
using System;

namespace PolicyService.Test.TestData
{
    public class PolicyFactory
    {
        internal static Policy AlreadyTerminatedPolicy()
        {
            var offer = OfferFactory.NewOfferValidUntil(DateTime.Now.AddDays(5));

            var policy = offer.Buy(PolicyHolderFactory.Abc());

            var terminationResult = policy.Terminate(DateTime.Now.AddDays(3));

            return policy;
        }
    }
}
