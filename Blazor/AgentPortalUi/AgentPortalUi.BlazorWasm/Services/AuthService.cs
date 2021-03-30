using AgentPortalUi.BlazorWasm.Contracts;
using AgentPortalUi.BlazorWasm.Contracts.Dto;
using AgentPortalUi.BlazorWasm.Model;
using RestEase;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AgentPortalUi.BlazorWasm.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthService client;

        public AuthService()
        {
            var httpClient = new HttpClient()
            {
                //TODO move url to some config
                BaseAddress = new Uri("http://localhost:6060/api/User")
            };

            client = RestClient.For<IAuthService>(httpClient);
        }

        public Task<AuthorizeResult> Authorize([Body] LoginModel loginModel)
        {
            return client.Authorize(loginModel);
        }
    }
}
