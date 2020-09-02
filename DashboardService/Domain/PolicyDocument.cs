using System;

namespace DashboardService.Domain
{
    public class PolicyDocument
    {
        public string Number { get; private set; }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public string PolicyHolder { get; private set; }
        public string ProductCode { get; private set; }
        public decimal TotalPremium { get; private set; }
        public string AgentLogin { get; private set; }
        
        public PolicyDocument(string number, DateTime @from, DateTime to, string policyHolder, string productCode, decimal totalPremium, string agentLogin)
        {
            Number = number;
            From = @from;
            To = to;
            PolicyHolder = policyHolder;
            ProductCode = productCode;
            TotalPremium = totalPremium;
            AgentLogin = agentLogin;
        }
    }
}