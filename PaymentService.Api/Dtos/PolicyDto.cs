using System;

namespace PaymentService.Api.Dto
{
    public class PolicyDto
    {
        public string Number { get; set; }
        private DateTime From { get; set; }
        private DateTime To { get; set; }
        private string PolicyHolder { get; set; }
    }
}
