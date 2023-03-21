using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PolicyService.Domain;

public class Price
{
    private readonly Dictionary<string, decimal> coverPrices;

    public Price(Dictionary<string, decimal> coverPrices)
    {
        this.coverPrices = coverPrices;
    }

    public IReadOnlyDictionary<string, decimal> CoverPrices => new ReadOnlyDictionary<string, decimal>(coverPrices);
}