using PolicyService.Domain;

namespace PolicyService.Test.Domain;

public class PolicyHolderFactory
{
    internal static PolicyHolder Abc()
    {
        return new PolicyHolder
        (
            "A", "B", "C",
            Address.Of("Poland", "00-133", "Warsaw", "Chłodna 52")
        );
    }
}