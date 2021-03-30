using AgentPortalUi.BlazorWasm.Contracts.Dto;
using AgentPortalUi.BlazorWasm.Model;
using RestEase;
using System.Threading.Tasks;

namespace AgentPortalUi.BlazorWasm.Contracts
{
    public interface IAuthService
    {
        [Post]
        Task<AuthorizeResult> Authorize(
            [Body] LoginModel loginModel
            );
    }
}
