using MediatR;
using PaymentService.Api.Dto;

namespace PaymentService.Api.Command
{
    public class PolicyRegisteredCommand : IRequest<bool>
    {
        public PolicyDto Policy { get; private set; }
    }
}
