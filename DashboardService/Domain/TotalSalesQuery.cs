using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DashboardService.Domain
{
    public class TotalSalesQuery
    {
        public string FilterByProductCode { get; }
        public DateTime FilterBySalesDateStart { get; }
        public DateTime FilterBySalesDateEnd { get; }

        public TotalSalesQuery(string filterByProductCode, DateTime filterBySalesDateStart, DateTime filterBySalesDateEnd)
        {
            FilterByProductCode = filterByProductCode;
            FilterBySalesDateStart = filterBySalesDateStart;
            FilterBySalesDateEnd = filterBySalesDateEnd;
        }
    }

    public class TotalSalesQueryResult
    {
        public SalesResult Total { get; private set; }
        
        private readonly IDictionary<string, SalesResult> perProductTotal = new Dictionary<string, SalesResult>();

        public IDictionary<string, SalesResult> PerProductTotal =>  new ReadOnlyDictionary<string, SalesResult>(perProductTotal);

        public TotalSalesQueryResult()
        {
            Total = SalesResult.NoSale();
        }

        public TotalSalesQueryResult ProductResult(string productCode, SalesResult result)
        {
            perProductTotal[productCode] = result;
            Total = new SalesResult
            (
            perProductTotal.Values.Select(r=>r.PoliciesCount).Sum(),
            perProductTotal.Values.Select(r=>r.PremiumAmount).Sum()
            );
            return this;
        } 
    }
}