using System;
using System.Collections.Generic;
using PricingService.Domain;
using Xunit;
using static Xunit.Assert;

namespace PricingService.Test.Domain;

public class TariffTest
{
    [Fact]
    public void CanCalculateTravelPolicyPrice()
    {
        var subject = new Dictionary<string, object>
        {
            ["NUM_OF_ADULTS"] = 1M,
            ["NUM_OF_CHILDREN"] = 1M,
            ["DESTINATION"] = "EUR"
        };

        var calculation = new Calculation(
            "TRI",
            DateTimeOffset.Now.AddDays(5),
            DateTimeOffset.Now.AddDays(10),
            new List<string> { "C1", "C2", "C3" },
            subject
        );

        var tariff = TariffFactory.Travel();

        var price = tariff.CalculatePrice(calculation);

        Equal(98M, price.TotalPremium);
        Equal(26M, price.Covers["C1"].Price);
        Equal(52M, price.Covers["C2"].Price);
        Equal(20M, price.Covers["C3"].Price);
    }

    [Fact]
    public void CanCalculateHousePolicyPrice()
    {
        var subject = new Dictionary<string, object>
        {
            ["TYP"] = "APT",
            ["AREA"] = 95M,
            ["NUM_OF_CLAIM"] = 1M,
            ["FLOOD"] = "NO"
        };

        var calculation = new Calculation(
            "HSI",
            DateTimeOffset.Now.AddDays(5),
            DateTimeOffset.Now.AddDays(5).AddYears(1),
            new List<string> { "C1", "C2", "C3" },
            subject
        );

        var tariff = TariffFactory.House();

        var price = tariff.CalculatePrice(calculation);

        Equal(172.50M, price.TotalPremium);
        Equal(118.75M, price.Covers["C1"].Price);
        Equal(23.75M, price.Covers["C2"].Price);
        Equal(30M, price.Covers["C3"].Price);
    }

    [Fact]
    public void CanCalculateCarPolicyPrice()
    {
        var subject = new Dictionary<string, object>
        {
            ["NUM_OF_CLAIM"] = 1M
        };

        var calculation = new Calculation(
            "CAR",
            DateTimeOffset.Now.AddDays(5),
            DateTimeOffset.Now.AddDays(5).AddYears(1),
            new List<string> { "C1" },
            subject
        );

        var tariff = TariffFactory.Car();

        var price = tariff.CalculatePrice(calculation);

        Equal(100M, price.TotalPremium);
        Equal(100M, price.Covers["C1"].Price);
    }
}