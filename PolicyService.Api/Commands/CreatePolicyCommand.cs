using MediatR;
using PolicyService.Api.Commands.Dtos;

namespace PolicyService.Api.Commands
{
    public class CreatePolicyCommand : IRequest<CreatePolicyResult>
    {
        public string OfferNumber { get; set; }
        public PersonDto PolicyHolder { get; set; }
        public AddressDto PolicyHolderAddress { get; set; }
    }
}
