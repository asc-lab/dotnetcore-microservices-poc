using MediatR;
using PricingService.Api.Commands.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace PricingService.Api.Commands
{
    public class CalculatePriceCommand : IRequest<CalculatePriceResult>
    {
        public string ProductCode { get; set; }
        public DateTimeOffset PolicyFrom { get; set; }
        public DateTimeOffset PolicyTo { get; set; }
        public List<string> SelectedCovers { get; set; }
        public List<QuestionAnswer> Answers { get; set; }
    }
}
