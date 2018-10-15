using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PolicyService.Domain
{
    public enum OfferStatus
    {
        New,
        Converted,
        Rejected
    }

    public class Offer
    {
        public virtual Guid? Id { get; protected set; }

        public virtual string Number { get; protected set; }

        //...
        public virtual string ProductCode { get; protected set; }

        public virtual ValidityPeriod PolicyValidityPeriod { get; protected set; }

        public virtual PolicyHolder PolicyHolder { get; protected set; }

        protected IList<Cover> covers = new List<Cover>();

        public virtual decimal TotalPrice { get; protected set; }

        //private Map<String, String> answers;
        
        public virtual OfferStatus Status { get; protected set; }

        public virtual DateTimeOffset CreateionDate { get; protected set; }


        public virtual IReadOnlyCollection<Cover> Covers => new ReadOnlyCollection<Cover>(covers);

        public static Offer ForPrice(
            String productCode,
            DateTimeOffset policyFrom,
            DateTimeOffset policyTo,
            PolicyHolder policyHolder,
            Price price)
        {
            return new Offer
            (
                productCode,
                policyFrom,
                policyTo,
                policyHolder,
                price
            );
        }

        public Offer(
            String productCode,
            DateTimeOffset policyFrom,
            DateTimeOffset policyTo,
            PolicyHolder policyHolder,
            Price price)
        {
            Id = null;
            Number = Guid.NewGuid().ToString();
            ProductCode = productCode;
            PolicyValidityPeriod = ValidityPeriod.Between(policyFrom, policyTo);
            PolicyHolder = policyHolder;
            covers = price.CoverPrices.Select(c => new Cover(c.Key, c.Value)).ToList();
            Status = OfferStatus.New;
            CreateionDate = DateTimeOffset.Now;
            TotalPrice = price.CoverPrices.Sum(c => c.Value);
        }

        protected Offer() { } //NH required

        public virtual void Convert()
        {

        }

        public virtual bool IsExpired(DateTimeOffset theDate)
        {
            return this.CreateionDate.AddDays(30) < theDate;
        }
    }
}
