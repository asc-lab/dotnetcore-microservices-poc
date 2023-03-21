using PolicyService.Domain;
using Xunit;

namespace PolicyService.Test.Domain;

public class OfferAssert
{
    private readonly Offer sut;

    private OfferAssert(Offer sut)
    {
        this.sut = sut;
    }

    public static OfferAssert AssertThat(Offer offer)
    {
        return new OfferAssert(offer);
    }

    public OfferAssert StatusIsNew()
    {
        Assert.Equal(OfferStatus.New, sut.Status);
        return this;
    }

    public OfferAssert StatusIsConverted()
    {
        Assert.Equal(OfferStatus.Converted, sut.Status);
        return this;
    }

    public OfferAssert PriceIs(decimal expectedPrice)
    {
        Assert.Equal(expectedPrice, sut.TotalPrice);
        return this;
    }

    public OfferAssert ProductCodeIs(string expectedProductCode)
    {
        Assert.Equal(expectedProductCode, sut.ProductCode);
        return this;
    }

    public OfferAssert AgentIs(string expectedAgent)
    {
        Assert.Equal(expectedAgent, sut.AgentLogin);
        return this;
    }
}