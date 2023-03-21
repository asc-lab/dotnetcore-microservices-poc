using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PolicyService.Domain;

public class PolicyVersion
{
    protected IList<PolicyCover> covers = new List<PolicyCover>();

    protected PolicyVersion()
    {
    } //NH

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
        covers = offer.Covers.Select(c => new PolicyCover(c, offer.PolicyValidityPeriod.Clone())).ToList();
        TotalPremiumAmount = covers.Sum(c => c.Premium);
    }

    public virtual Guid? Id { get; protected set; }

    public virtual Policy Policy { get; protected set; }

    public virtual int VersionNumber { get; protected set; }

    public virtual PolicyHolder PolicyHolder { get; protected set; }

    public virtual ValidityPeriod CoverPeriod { get; protected set; }

    public virtual ValidityPeriod VersionValidityPeriod { get; protected set; }

    public virtual IReadOnlyCollection<PolicyCover> Covers => new ReadOnlyCollection<PolicyCover>(covers);

    public virtual decimal TotalPremiumAmount { get; protected set; }

    public static PolicyVersion FromOffer(
        Policy policy,
        int version,
        PolicyHolder policyHolder,
        Offer offer)
    {
        return new PolicyVersion(policy, version, policyHolder, offer);
    }

    public virtual bool IsEffectiveOn(DateTime theDate)
    {
        return VersionValidityPeriod.Contains(theDate);
    }

    public virtual PolicyVersion EndOn(DateTime endDate)
    {
        var endedCovers = covers.Select(c => c.EndOn(endDate)).ToList();

        var termVersion = new PolicyVersion
        {
            Policy = Policy,
            VersionNumber = Policy.NextVersionNumber(),
            PolicyHolder = new PolicyHolder(PolicyHolder.FirstName, PolicyHolder.LastName, PolicyHolder.Pesel,
                PolicyHolder.Address),
            CoverPeriod = CoverPeriod.EndOn(endDate),
            VersionValidityPeriod = ValidityPeriod.Between(endDate.AddDays(1), VersionValidityPeriod.ValidTo),
            covers = endedCovers,
            TotalPremiumAmount = endedCovers.Sum(c => c.Premium)
        };
        return termVersion;
    }
}