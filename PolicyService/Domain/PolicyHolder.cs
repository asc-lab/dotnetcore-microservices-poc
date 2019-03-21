using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.Domain
{
    public class PolicyHolder
    {
        public virtual string FirstName { get; protected set; }
        public virtual string LastName { get; protected set; }
        public virtual string Pesel { get; protected set; }
        public virtual Address Address { get; protected set; }

        public PolicyHolder(string firstName, string lastName, string pesel, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            Pesel = pesel;
            Address = address;
        }

        protected PolicyHolder() { } //NH required
    }
}
