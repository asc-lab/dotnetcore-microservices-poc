using System;
using PolicyService.Domain;
using PolicyService.Test.TestData;
using Xunit;
using static Xunit.Assert;

namespace PolicyService.Test.Domain
{
    public class PolicyTerminationTest
    {
        [Fact]
        public void CanTerminateActivePolicyInTheMiddleOfCoverPeriod()
        {
            var offer = OfferFactory.NewOfferValidUntil(DateTime.Now.AddDays(5));

            var policy = offer.Buy(PolicyHolderFactory.Abc());

            var terminationResult = policy.Terminate(DateTime.Now.AddDays(3));
            
            Equal(PolicyStatus.Terminated, policy.Status);
            Equal(180M, terminationResult.TerminalVersion.TotalPremiumAmount);
            Equal(120M, terminationResult.AmountToReturn);
        }

        [Fact]
        public void CannotTerminateTerminatedPolicy()
        {
            var policy = PolicyFactory.AlreadyTerminatedPolicy();

            Exception ex = Throws<ApplicationException>(() => policy.Terminate(DateTime.Now));
            Equal($"Policy {policy.Number} is already terminated", ex.Message);
        }
    }
}