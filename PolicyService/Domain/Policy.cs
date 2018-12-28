using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Util;

namespace PolicyService.Domain
{
    public enum PolicyStatus
    {
        Active,
        Terminated
    }

    public class Policy
    {
        public virtual Guid? Id { get; protected set; }

        public virtual string Number { get; protected set; }

        public virtual string ProductCode { get; protected set; }

        protected IList<PolicyVersion> versions = new List<PolicyVersion>();

        public virtual IReadOnlyCollection<PolicyVersion> Versions => new ReadOnlyCollection<PolicyVersion>(versions);

        public virtual PolicyStatus Status { get; protected set; }

        public virtual DateTime CreateionDate { get; protected set; }

        protected Policy() { } //NH constuctor
        
        public Policy(PolicyHolder policyHolder, Offer offer)
        {
            Id = null;
            Number = Guid.NewGuid().ToString();
            ProductCode = offer.ProductCode;
            Status = PolicyStatus.Active;
            CreateionDate = SysTime.CurrentTime;
            versions.Add(PolicyVersion.FromOffer(this,1,policyHolder,offer));
        }

        public virtual PolicyVersion Terminate(DateTime terminationDate)
        {
            //ensure is not already terminated
            if (Status != PolicyStatus.Active)
                throw new ApplicationException($"Policy {Number} is already terminated");

            //get version valid at term date
            var versionAtTerminationDate = versions.EffectiveOn(terminationDate);
            
            if (versionAtTerminationDate==null)
                throw new ApplicationException($"No valid policy {Number} version exists at {terminationDate}. Policy cannot be terminated.");

            if (!versionAtTerminationDate.CoverPeriod.Contains(terminationDate))
                throw new ApplicationException($"Policy {Number} does not cover {terminationDate}. Policy cannot be terminated at this date.");
            
            //create terminal version
            versions.Add(versionAtTerminationDate.EndOn(terminationDate));
            
            //change status
            Status = PolicyStatus.Terminated;
            
            //return term version
            return versions.LastVersion();
        }

        public virtual int NextVersionNumber()
        {
            return versions.Max(v => v.VersionNumber) + 1;
        }
    }
}
