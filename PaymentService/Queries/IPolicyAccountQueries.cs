using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentService.Api.Queries.Dtos;
using PaymentService.Domain;

namespace PaymentService.Queries
{
    public interface IPolicyAccountQueries
    {
        Task<IEnumerable<PolicyAccountDto>> FindAll();
        Task<PolicyAccountBalanceDto> FindByNumber(string accountNumber);
    }
}