using System;
using System.Collections.Generic;
using PolicyService.Domain;
using Xunit;
using static Xunit.Assert;

namespace PolicyService.Test.Domain
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

            OfferAssert
                .AssertThat(offer)
                .ProductCodeIs("P1")
                .StatusIsNew()
                .PriceIs(300M)
                .AgentIs(null);
        }
        
        [Fact]
        public void CanCreateOfferOnBehalfOfAgentBasedOnPrice()
        {
            var price = new Price(new Dictionary<string, decimal>
            {
                ["C1"] = 100M,
                ["C2"] = 200M
            });

            var offer = Offer.ForPriceAndAgent
            (
                "P1",
                DateTime.Now,
                DateTime.Now.AddDays(5),
                null,
                price,
                "jimmy.son"
            );

            OfferAssert
                .AssertThat(offer)
                .ProductCodeIs("P1")
                .StatusIsNew()
                .PriceIs(300M)
                .AgentIs("jimmy.son");
        }

        [Fact]
        public void CanBuyNewNonExpiredOffer()
        {
            var offer = OfferFactory.NewOfferValidUntil(DateTime.Now.AddDays(5));

            var policy = offer.Buy(PolicyHolderFactory.Abc());

            OfferAssert
                .AssertThat(offer)
                .StatusIsConverted();

            PolicyAssert
                .AssertThat(policy)
                .StatusIsActive()
                .HasVersions(1)
                .HasVersion(1)
                .AgentIs(null);

            PolicyVersionAssert
                .AssertThat(policy.Versions.WithNumber(1))
                .TotalPremiumIs(offer.TotalPrice);
        }
        
        [Fact]
        public void CanBuyNewNonExpiredOfferFromAgent()
        {
            var offer = OfferFactory.NewOfferValidUntilForAgent(DateTime.Now.AddDays(5),"jimmy.young");

            var policy = offer.Buy(PolicyHolderFactory.Abc());

            OfferAssert
                .AssertThat(offer)
                .StatusIsConverted()
                .AgentIs("jimmy.young");

            PolicyAssert
                .AssertThat(policy)
                .StatusIsActive()
                .HasVersions(1)
                .HasVersion(1);

            PolicyVersionAssert
                .AssertThat(policy.Versions.WithNumber(1))
                .TotalPremiumIs(offer.TotalPrice);
        }

        [Fact]
        public void CannotBuyAlreadyConvertedOffer()
        {
            var offer = OfferFactory.AlreadyConvertedOffer();

            Exception ex = Throws<ApplicationException>(() => offer.Buy(PolicyHolderFactory.Abc()));
            Equal($"Offer {offer.Number} is not in new status and cannot be bought", ex.Message);
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