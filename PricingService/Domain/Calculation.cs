using System;
using System.Collections.Generic;
using System.Linq;
using DynamicExpresso;
using PricingService.Extensions;

namespace PricingService.Domain
{
    public class Calculation
    {
        public string ProductCode { get; private set; }
        public DateTimeOffset PolicyFrom { get; private set; }
        public DateTimeOffset PolicyTo { get; private set; }
        public decimal TotalPremium { get; private set; }
        public Dictionary<string, Cover> Covers { get; private set; } = new Dictionary<string, Cover>();
        public Dictionary<string, object> Subject { get; private set; } = new Dictionary<string, object>();

        public void UpdateTotal()
        {
            TotalPremium = Covers.Values.Sum(c => c.Price);
        }

        public Calculation(
            string productCode,
            DateTimeOffset policyFrom,
            DateTimeOffset policyTo,
            IEnumerable<string> selectedCovers,
            Dictionary<string,object> subject
            )
        {
            ProductCode = productCode;
            PolicyFrom = policyFrom;
            PolicyTo = policyTo;
            TotalPremium = 0M;
            selectedCovers.ForEach(ZeroPrice);
            Subject = subject;
        }

        public (List<Parameter>, List<object>) ToCalculationParams()
        {
            var parameters = new List<Parameter>();
            var values = new List<object>();

            parameters.Add(new Parameter("policyFrom", typeof(DateTimeOffset)));
            values.Add(PolicyFrom);
            parameters.Add(new Parameter("policyTo", typeof(DateTimeOffset)));
            values.Add(PolicyTo);
            
            foreach (var cover in Covers)
            {
                parameters.Add(new Parameter(cover.Key, typeof(Cover)));
                values.Add(cover.Value);
            }

            foreach (var question in Subject)
            {
                parameters.Add(new Parameter(question.Key, question.Value.GetType()));
                values.Add(question.Value);
            }

            return (parameters, values);
        }

        private void ZeroPrice(string coverCode)
        {
            Covers.Add(coverCode, new Cover(coverCode, 0M));
        }
    }
}
