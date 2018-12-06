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

        public virtual PolicyVersion Version(int number)
        {
            return Versions.First(v => v.VersionNumber == number);
        }
    }
}
