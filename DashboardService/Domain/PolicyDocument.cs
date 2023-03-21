using System;

namespace DashboardService.Domain;

public class PolicyDocument
{
    public PolicyDocument(string number, DateTime from, DateTime to, string policyHolder, string productCode,
        decimal totalPremium, string agentLogin)
    {
        Number = number;
        From = from;
        To = to;
        PolicyHolder = policyHolder;
        ProductCode = productCode;
        TotalPremium = totalPremium;
        AgentLogin = agentLogin;
    }

    public string Number { get; }
    public DateTime From { get; }
    public DateTime To { get; }
    public string PolicyHolder { get; }
    public string ProductCode { get; }
    public decimal TotalPremium { get; }
    public string AgentLogin { get; }
}