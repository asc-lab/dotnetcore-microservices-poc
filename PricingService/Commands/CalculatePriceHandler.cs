using FluentValidation;
using MediatR;
using PricingService.Api.Commands;
using PricingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PricingService.Commands
{
    public class CalculatePriceHandler : IRequestHandler<CalculatePriceCommand, CalculatePriceResult>
    {
        private IUnitOfWork unitOfWork;
        private CalculatePriceCommandValidator commandValidator = new CalculatePriceCommandValidator(); 

        public CalculatePriceHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<CalculatePriceResult> Handle(CalculatePriceCommand cmd, CancellationToken cancellationToken)
        {
            commandValidator.ValidateAndThrow(cmd);

            Tariff tariff = unitOfWork.Tariffs.WithCode(cmd.ProductCode);

            var calculation = tariff.CalculatePrice(ToCalculation(cmd));

            return ToResult(calculation);
        }

        private Calculation ToCalculation(CalculatePriceCommand cmd)
        {
            return new Calculation(
                cmd.ProductCode,
                cmd.PolicyFrom,
                cmd.PolicyTo,
                cmd.SelectedCovers,
                cmd.Answers.ToDictionary(a => a.QuestionCode, a=> a.GetAnswer()));
        }

        private CalculatePriceResult ToResult(Calculation calculation)
        {
            return new CalculatePriceResult
            {
                TotalPrice = calculation.TotalPremium,
                CoverPrices = calculation.Covers.ToDictionary(c => c.Key, c => c.Value.Price)
            };
        }
    }
}
