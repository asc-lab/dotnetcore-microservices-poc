using Xunit;

namespace PricingService.IntegrationTest;

[CollectionDefinition("PricingControllerFixtureCollection")]
public class PricingControllerFixtureCollection : ICollectionFixture<PricingControllerFixture>
{
}