using System;
using System.Collections.Generic;
using System.Linq;
using PolicyService.Domain;
using Xunit;
using static Xunit.Assert;

namespace PolicyService.Tests.Domain
{
    public class OfferTest
    {
        [Fact]
        public void CanCreateOfferBasedOnPrice()
        {
            var price = new Price(new Dictionary<string, decimal>()
            {
                ["C1"] = 100M,
                ["C2"] = 200M
            });

            var offer = Offer.ForPrice
            (
                "P1",
                DateTime.Now,
                DateTime.Now.AddDays(5),
                null,
                price
            );

            Equal("P1", offer.ProductCode);
            Equal(OfferStatus.New, offer.Status);     
            Equal(300M, offer.TotalPrice);
        }

        [Fact]
        public void CanBuyNewNonExpiredOffer()
        {
            var offer = OfferFactory.NewOfferValidUntil(DateTime.Now.AddDays(5));

            var policy = offer.Buy(PolicyHolderFactory.Abc());
            
            Equal(OfferStatus.Converted, offer.Status);
            Equal(offer.TotalPrice, policy.Versions.FirstVersion().TotalPremiumAmount);
        }

        [Fact]
        public void CannotBuyAlreadyConvertedOffer()
        {
            var offer = OfferFactory.AlreadyConvertedOffer();

            Exception ex = Throws<ApplicationException>(() => offer.Buy(PolicyHolderFactory.Abc()));
            Equal($"Offer {offer.Number} is not in new status and connot be bought", ex.Message);
        }

        [Fact]
        public void CannotBuyExpiredOffer()
        {
            var offer = OfferFactory.NewOfferValidUntil(DateTime.Now.AddDays(-5));
            Exception ex = Throws<ApplicationException>(() => offer.Buy(PolicyHolderFactory.Abc()));
            Equal($"Offer {offer.Number} has expired", ex.Message);
        }
    }
}