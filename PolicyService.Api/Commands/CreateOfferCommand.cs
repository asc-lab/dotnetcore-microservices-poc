using MediatR;
using PolicyService.Api.Commands.Dtos;
using System;
using System.Collections.Generic;
using System.Text;


namespace PolicyService.Api.Commands
{
    public class CreateOfferCommand : IRequest<CreateOfferResult>
    {
        public string ProductCode { get; set; }
        public DateTimeOffset PolicyFrom { get; set; }
        public DateTimeOffset PolicyTo { get; set; }
        public List<string> SelectedCovers { get; set; }
        public List<QuestionAnswer> Answers { get; set; }
    }
}
