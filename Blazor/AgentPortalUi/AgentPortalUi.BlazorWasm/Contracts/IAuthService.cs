using AgentPortalUi.BlazorWasm.Dto;
using RestEase;
using System.Threading.Tasks;

namespace AgentPortalUi.BlazorWasm.Contracts
{
    public interface IAuthService
    {
        [Post]
        Task<string> Authorize(
            [Body] LoginModel loginModel
            );
    }
}
