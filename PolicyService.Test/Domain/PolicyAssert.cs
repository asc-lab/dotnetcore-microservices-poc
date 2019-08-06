using System.Linq;
using PolicyService.Domain;
using Xunit;

namespace PolicyService.Test.Domain
{
    public class PolicyAssert
    {
        private readonly Policy sut;

        private PolicyAssert(Policy sut)
        {
            this.sut = sut;
        }

        public static PolicyAssert AssertThat(Policy policy)
        {
            return new PolicyAssert(policy);
        }

        public PolicyAssert HasVersions(int expectedVersionsCount)
        {
            Assert.Equal(expectedVersionsCount, sut.Versions.Count);
            return this;
        }
        
        public PolicyAssert HasVersion(int versionNumber)
        {
            Assert.NotNull(sut.Versions.FirstOrDefault(v => v.VersionNumber==versionNumber));
            return this;
        }

        public PolicyAssert StatusIsActive()
        {
            Assert.Equal(PolicyStatus.Active, sut.Status);
            return this;
        }
        
        public PolicyAssert StatusIsTerminated()
        {
            Assert.Equal(PolicyStatus.Terminated, sut.Status);
            return this;
        }
        
        public PolicyAssert AgentIs(string agent)
        {
            Assert.Equal(agent, sut.AgentLogin);
            return this;
        }
    }


    public class PolicyVersionAssert
    {
        private readonly PolicyVersion sut;

        private PolicyVersionAssert(PolicyVersion sut)
        {
            this.sut = sut;
        }

        public static PolicyVersionAssert AssertThat(PolicyVersion policyVersion)
        {
            return new PolicyVersionAssert(policyVersion);
        }
        
        public PolicyVersionAssert TotalPremiumIs(decimal expectedPremium)
        {
            Assert.Equal(expectedPremium, sut.TotalPremiumAmount);
            return this;
        }
    }
}