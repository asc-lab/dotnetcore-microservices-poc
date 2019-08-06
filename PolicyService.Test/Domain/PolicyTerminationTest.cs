using System;
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

            PolicyAssert
                .AssertThat(policy)
                .HasVersions(2)
                .HasVersion(2)
                .StatusIsTerminated();

            PolicyVersionAssert
                .AssertThat(terminationResult.TerminalVersion)
                .TotalPremiumIs(180M);
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