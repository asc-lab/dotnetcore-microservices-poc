using AgentPortalUi.BlazorWasm.Contracts;
using AgentPortalUi.BlazorWasm.Dto;
using System;
using System.Threading.Tasks;

namespace AgentPortalUi.BlazorWasm.Services
{
    public class UserService
    {
        public string Token { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

        private IAuthService authService;
        private ILocalStorageService localStorageService;

        public UserService(IAuthService authService, ILocalStorageService localStorageService)
        {
            this.authService = authService;
            this.localStorageService = localStorageService;
        }

        public async Task Initialize()
        {
            Token = await localStorageService.GetItem<string>("token");
        }

        public async Task<bool> LogIn(LoginModel model)
        {
            try
            {
                Token = await authService.Authorize(model);
                //ParseClaimsFromJwt(Token);
                await localStorageService.SetItem("token", Token);
            }
            catch (Exception ex)
            {
                Token = null;
                await localStorageService.RemoveItem("token");
            }

            return IsAuthenticated;
        }

        //private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        //{
        //    var payload = jwt.Split('.')[1];
        //    var jsonBytes = ParseBase64WithoutPadding(payload);
        //    var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        //    return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        //}

        //private byte[] ParseBase64WithoutPadding(string base64)
        //{
        //    switch (base64.Length % 4)
        //    {
        //        case 2: base64 += "=="; break;
        //        case 3: base64 += "="; break;
        //    }
        //    return Convert.FromBase64String(base64);
        //}
    }
}
