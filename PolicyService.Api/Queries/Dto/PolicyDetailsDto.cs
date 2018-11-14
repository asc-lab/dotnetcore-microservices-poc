using System;
using System.Collections.Generic;

namespace PolicyService.Api.Queries.Dto
{
    public class PolicyDetailsDto
    {
        public string Number { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public DateTimeOffset DateTo { get; set; }
        public string PolicyHolder { get; set; }
        public decimal TotalPremium { get; set; }
        public string ProductCode { get; set; }
        public string AccountNumber { get; set; }

        public List<string> Covers { get; set; }
    }
}