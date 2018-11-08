using PolicyService.Domain;

namespace PolicyService.Tests.Domain
{
    public class PolicyHolderFactory
    {
        internal static PolicyHolder Abc()
        {
            return new PolicyHolder("A","B","C");
        }
    }
}