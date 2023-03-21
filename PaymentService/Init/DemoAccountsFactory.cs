using System.Collections.Generic;
using PaymentService.Domain;

namespace PaymentService.Init;

internal static class DemoAccountsFactory
{
    internal static List<PolicyAccount> DemoAccounts()
    {
        return new List<PolicyAccount>
        {
            new("POLICY_1", "231232132131", "Tim", "Jones"),
            new("POLICY_2", "389hfswjfrh2032r", "Mike", "Zorn"),
            new("POLICY_3", "0rju130fhj20", "Judith", "Powell")
        };
    }
}