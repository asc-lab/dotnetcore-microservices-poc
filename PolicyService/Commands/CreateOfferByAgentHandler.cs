using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PolicyService.Api.Commands;
using PolicyService.Domain;

namespace PolicyService.Commands
{
    public class CreateOfferByAgentHandler : IRequestHandler<CreateOfferByAgentCommand, CreateOfferResult>
    {
        private readonly IUnitOfWorkProvider uowProvider;
        private readonly IPricingService pricingService;

        public CreateOfferByAgentHandler(IUnitOfWorkProvider uowProvider, IPricingService pricingService)
        {
            this.uowProvider = uowProvider;
            this.pricingService = pricingService;
        }

        public async Task<CreateOfferResult> Handle(CreateOfferByAgentCommand request, CancellationToken cancellationToken)
        {
            //calculate price
            var priceParams = ConstructPriceParams(request);
            var price = await pricingService.CalculatePrice(priceParams);

            
            var o = Offer.ForPriceAndAgent(
                priceParams.ProductCode,
                priceParams.PolicyFrom,
                priceParams.PolicyTo,
                null,
                price,
                request.AgentLogin
            );

            //create and save offer
            using (var uow = uowProvider.Create())
            {
                uow.Offers.Add(o);
                await uow.CommitChanges();

                //return result
                return ConstructResult(o);
            }
        }

        private CreateOfferResult ConstructResult(Offer o)
        {
            return new CreateOfferResult
            {
                OfferNumber = o.Number,
                TotalPrice = o.TotalPrice,
                CoversPrices = o.Covers.ToDictionary(c => c.Code, c => c.Price)
            };
        }

        private PricingParams ConstructPriceParams(CreateOfferCommand request)
        {
            return new PricingParams
            {
                ProductCode = request.ProductCode,
                PolicyFrom = request.PolicyFrom,
                PolicyTo = request.PolicyTo,
                SelectedCovers = request.SelectedCovers,
                Answers = request.Answers.Select(a => Answer.Create(a.QuestionType, a.QuestionCode, a.GetAnswer())).ToList()
            };
        }
        
    }
}