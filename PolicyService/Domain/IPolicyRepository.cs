using System.Threading.Tasks;

namespace PolicyService.Domain;

public interface IPolicyRepository
{
    void Add(Policy policy);

    Task<Policy> WithNumber(string number);
}