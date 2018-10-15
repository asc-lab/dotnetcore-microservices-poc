using PricingService.Api.Commands;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.Domain
{
    public interface IPricingService
    {
        Task<Price> CalculatePrice(PricingParams pricingParams);
    }

    public class PricingParams
    {
        public string ProductCode { get; set; }
        public DateTimeOffset PolicyFrom { get; set; }
        public DateTimeOffset PolicyTo { get; set; }
        public List<string> SelectedCovers { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
