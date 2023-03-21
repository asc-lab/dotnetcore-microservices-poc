using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using PricingService.Api.Commands;
using PricingService.Domain;

namespace PricingService.Commands;

public class CalculatePriceHandler : IRequestHandler<CalculatePriceCommand, CalculatePriceResult>
{
    private readonly CalculatePriceCommandValidator commandValidator = new();
    private readonly IDataStore dataStore;

    public CalculatePriceHandler(IDataStore dataStore)
    {
        this.dataStore = dataStore;
    }

    public async Task<CalculatePriceResult> Handle(CalculatePriceCommand cmd, CancellationToken cancellationToken)
    {
        commandValidator.ValidateAndThrow(cmd);

        var tariff = await dataStore.Tariffs[cmd.ProductCode];

        var calculation = tariff.CalculatePrice(ToCalculation(cmd));

        return ToResult(calculation);
    }

    private static Calculation ToCalculation(CalculatePriceCommand cmd)
    {
        return new Calculation(
            cmd.ProductCode,
            cmd.PolicyFrom,
            cmd.PolicyTo,
            cmd.SelectedCovers,
            cmd.Answers.ToDictionary(a => a.QuestionCode, a => a.GetAnswer()));
    }

    private static CalculatePriceResult ToResult(Calculation calculation)
    {
        return new CalculatePriceResult
        {
            TotalPrice = calculation.TotalPremium,
            CoverPrices = calculation.Covers.ToDictionary(c => c.Key, c => c.Value.Price)
        };
    }
}