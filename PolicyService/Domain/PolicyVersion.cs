using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.Domain
{
    public class PolicyVersion
    {
        public virtual Guid? Id { get; protected set; }

        public virtual Policy Policy { get; protected set; }

        public virtual int VersionNumber { get; protected set; }

        public virtual PolicyHolder PolicyHolder { get; protected set; }

        public virtual ValidityPeriod CoverPeriod { get; protected set; }

        public virtual ValidityPeriod VersionValidityPeriod { get; protected set; }

        protected IList<Cover> covers = new List<Cover>();

        public virtual IReadOnlyCollection<Cover> Covers => new ReadOnlyCollection<Cover>(covers);

        public virtual decimal TotalPremiumAmount { get; protected set; }

        protected PolicyVersion() { } //NH

        public static PolicyVersion FromOffer(
            Policy policy,
            int version,
            PolicyHolder policyHolder,
            Offer offer)
        {
            return new PolicyVersion(policy,version,policyHolder,offer);
        }

        private PolicyVersion(
            Policy policy,
            int version,
            PolicyHolder policyHolder,
            Offer offer)
        {
            Policy = policy;
            VersionNumber = version;
            PolicyHolder = policyHolder;
            CoverPeriod = offer.PolicyValidityPeriod.Clone();
            VersionValidityPeriod = offer.PolicyValidityPeriod.Clone();
            covers = offer.Covers.Select(c => c.Clone()).ToList();
            TotalPremiumAmount = covers.Sum(c => c.Price);
        }
    }
}
