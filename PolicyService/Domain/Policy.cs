using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PolicyService.Domain;

public enum PolicyStatus
{
    Active,
    Terminated
}

public class Policy
{
    protected IList<PolicyVersion> versions = new List<PolicyVersion>();

    protected Policy()
    {
    } //NH constuctor

    protected Policy(PolicyHolder policyHolder, Offer offer)
    {
        Id = null;
        Number = Guid.NewGuid().ToString();
        ProductCode = offer.ProductCode;
        Status = PolicyStatus.Active;
        CreationDate = SysTime.CurrentTime;
        AgentLogin = offer.AgentLogin;
        versions.Add(PolicyVersion.FromOffer(this, 1, policyHolder, offer));
    }

    public virtual Guid? Id { get; protected set; }

    public virtual string Number { get; protected set; }

    public virtual string ProductCode { get; protected set; }

    public virtual IReadOnlyCollection<PolicyVersion> Versions => new ReadOnlyCollection<PolicyVersion>(versions);

    public virtual PolicyStatus Status { get; protected set; }

    public virtual DateTime CreationDate { get; protected set; }

    public virtual string AgentLogin { get; protected set; }

    public static Policy FromOffer(PolicyHolder policyHolder, Offer offer)
    {
        return new Policy(policyHolder, offer);
    }

    public virtual PolicyTerminationResult Terminate(DateTime terminationDate)
    {
        //ensure is not already terminated
        if (Status != PolicyStatus.Active)
            throw new ApplicationException($"Policy {Number} is already terminated");

        //get version valid at term date
        var versionAtTerminationDate = versions.EffectiveOn(terminationDate);

        if (versionAtTerminationDate == null)
            throw new ApplicationException(
                $"No valid policy {Number} version exists at {terminationDate}. Policy cannot be terminated.");

        if (!versionAtTerminationDate.CoverPeriod.Contains(terminationDate))
            throw new ApplicationException(
                $"Policy {Number} does not cover {terminationDate}. Policy cannot be terminated at this date.");

        //create terminal version
        versions.Add(versionAtTerminationDate.EndOn(terminationDate));

        //change status
        Status = PolicyStatus.Terminated;

        //return term version
        var terminalVersion = versions.LastVersion();
        return new PolicyTerminationResult(terminalVersion,
            versionAtTerminationDate.TotalPremiumAmount - terminalVersion.TotalPremiumAmount);
    }

    public virtual int NextVersionNumber()
    {
        return versions.Count == 0 ? 1 : versions.LastVersion().VersionNumber + 1;
    }
}