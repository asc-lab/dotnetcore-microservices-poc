using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Api.Dto
{
    public class PolicyAccountBalanceDto
    {
        private string PolicyAccountNumber { get; set; }
        private string PolicyNumber { get; set; }
        private decimal Balance { get; set; }
    }
}
