using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DashboardService.Domain;

public class AgentSalesQuery
{
    public AgentSalesQuery(string filterByAgentLogin, string filterByProductCode, DateTime filterBySalesDateStart,
        DateTime filterBySalesDateEnd)
    {
        FilterByAgentLogin = filterByAgentLogin;
        FilterByProductCode = filterByProductCode;
        FilterBySalesDateStart = filterBySalesDateStart;
        FilterBySalesDateEnd = filterBySalesDateEnd;
    }

    public string FilterByAgentLogin { get; }
    public string FilterByProductCode { get; }
    public DateTime FilterBySalesDateStart { get; }
    public DateTime FilterBySalesDateEnd { get; }
}

public class AgentSalesQueryResult
{
    private readonly IDictionary<string, SalesResult> perAgentTotal = new Dictionary<string, SalesResult>();

    public IDictionary<string, SalesResult> PerAgentTotal => new ReadOnlyDictionary<string, SalesResult>(perAgentTotal);

    public AgentSalesQueryResult AgentResult(string agentLogin, SalesResult result)
    {
        perAgentTotal[agentLogin] = result;
        return this;
    }
}