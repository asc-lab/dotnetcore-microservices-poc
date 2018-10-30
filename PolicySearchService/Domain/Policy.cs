using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySearchService.Domain
{
    public class Policy
    {
        public string PolicyNumber { get; set; }
        public DateTimeOffset PolicyStartDate { get; set; }
        public DateTimeOffset PolicyEndDate { get; set; }
        public string ProductCode { get; set; }
        public string PolicyHolder { get; set; }
        public decimal PremiumAmount { get; set; }
    }
}
