using MediatR;
using PolicyService.Api.Commands.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolicyService.Api.Commands
{
    public class CreatePolicyCommand : IRequest<CreatePolicyResult>
    {
        public string OfferNumber { get; set; }
        public PersonDto PolicyHolder { get; set; }
        public AddressDto PolicyHolderAddress { get; set; }
    }
}
