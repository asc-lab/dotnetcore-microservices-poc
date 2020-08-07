using System;
using DashboardService.Domain;

namespace DashboardService.Test
{
    public class PolicyDocumentBuilder
    {
        private string number;
        private DateTime from;
        private DateTime to;
        private string policyHolder;
        private string productCode;
        private decimal totalPremium;
        private string agentLogin;
        
        public static PolicyDocumentBuilder Create() => new PolicyDocumentBuilder();

        public PolicyDocumentBuilder()
        {
            number = Guid.NewGuid().ToString();
            from = new DateTime(2020,1,1);
            to = from.AddYears(1).AddDays(-1);
            policyHolder = "Jan Test";
            productCode = "TRI";
            totalPremium = 100M;
            agentLogin = "jimmy.solid";
        }

        public PolicyDocumentBuilder WithNumber(string policyNumber)
        {
            number = policyNumber;
            return this;
        }
        
        public PolicyDocumentBuilder WithDates(string start, string end)
        {
            from = DateTime.Parse(start);
            to = DateTime.Parse(end);
            return this;
        }

        public PolicyDocumentBuilder WithProduct(string product)
        {
            productCode = product;
            return this;
        }

        public PolicyDocumentBuilder WithPremium(decimal premium)
        {
            totalPremium = premium;
            return this;
        }
        
        public PolicyDocumentBuilder WithAgent(string agent)
        {
            agentLogin = agent;
            return this;
        }
        
        public PolicyDocument Build()
        {
            return new PolicyDocument
            (
                number,
                @from,
                to,
                policyHolder,
                productCode,
                totalPremium,
                agentLogin
            );
        }
    }
}