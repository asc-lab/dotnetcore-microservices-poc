using FluentValidation;
using Moq;
using PricingService.Api.Commands;
using PricingService.Api.Commands.Dto;
using PricingService.Commands;
using PricingService.Domain;
using PricingService.Test.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PricingService.Test.Commands
{
    public class CalculatePriceHandlerShould
    {
        [Fact]
        public async Task ThrowExceptionsIfTheCommandIsDefault() {
            var command = new CalculatePriceCommand();
            var handler = new CalculatePriceHandler(new Mock<IDataStore>().Object);

            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.True(exception.Errors.Count() > 0);
        }

        [Fact]
        public async Task ThrowAnExceptionIfTheCommandsProductCodeIsEmpty() {
            var command = new CalculatePriceCommand { SelectedCovers = new List<string>(), Answers = new List<QuestionAnswer>() };
            var handler = new CalculatePriceHandler(new Mock<IDataStore>().Object);

            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.Single(exception.Errors);
            Assert.Equal("ProductCode", exception.Errors.First().PropertyName);
        }

        [Fact]
        public async Task ThrowAnExceptionIfTheCommandsSelectedCoversIsEmpty() {
            var command = new CalculatePriceCommand { ProductCode = "test", Answers = new List<QuestionAnswer>() };
            var handler = new CalculatePriceHandler(new Mock<IDataStore>().Object);

            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.Single(exception.Errors);
            Assert.Equal("SelectedCovers", exception.Errors.First().PropertyName);
        }

        [Fact]
        public async Task ThrowAnExceptionIfTheCommandsAnswersIsEmpty() {
            var command = new CalculatePriceCommand { ProductCode = "test", SelectedCovers = new List<string>() };
            var handler = new CalculatePriceHandler(new Mock<IDataStore>().Object);

            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));

            Assert.NotNull(exception);
            Assert.Single(exception.Errors);
            Assert.Equal("Answers", exception.Errors.First().PropertyName);
        }

        [Fact]
        public async Task ReturnCorrectPrice() {
            var tariff = TariffFactory.Travel();
            var command = new CalculatePriceCommand {
                ProductCode = tariff.Code,
                PolicyFrom = DateTimeOffset.Now.AddDays(5),
                PolicyTo = DateTimeOffset.Now.AddDays(10),
                SelectedCovers = new List<string> { "C1", "C2", "C3" },
                Answers = new List<QuestionAnswer>
                {
                    new NumericQuestionAnswer { QuestionCode = "NUM_OF_ADULTS", Answer = 1M},
                    new NumericQuestionAnswer { QuestionCode = "NUM_OF_CHILDREN", Answer = 1M},
                    new TextQuestionAnswer { QuestionCode = "DESTINATION", Answer = "EUR"}
                }
            };
            var dataStoreMock = new Mock<IDataStore>();
            dataStoreMock.Setup(x => x.Tariffs[command.ProductCode]).ReturnsAsync(tariff);

            var handler = new CalculatePriceHandler(dataStoreMock.Object);

            var calculatePriceResult = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(calculatePriceResult);
            Assert.Equal(98M, calculatePriceResult.TotalPrice);
        }
    }
}
