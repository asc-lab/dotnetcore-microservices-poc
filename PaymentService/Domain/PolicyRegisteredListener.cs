using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Domain
{
    public class PolicyRegisteredListener
    {
        private readonly IPolicyAccountRepository policyAccountRepository;
        private readonly PolicyAccountNumberGenerator policyAccountNumberGenerator;

        //public void OnPolicyRegistered(PolicyRegisteredEvent @event) {
        //    PolicyAccount accountOpt = policyAccountRepository.findForPolicy(@event.Policy.Number);

        //    if (accountOpt != null)
        //        CreateAccount(@event.Policy);
        //}

        //public void CreateAccount(PolicyDto policy)
        //{
        //    policyAccountRepository.Add(new PolicyAccount(policy.Number, policyAccountNumberGenerator.Generate));
        //}
    }
}
