using AgentPortalUi.BlazorWasm.Contracts;
using AgentPortalUi.BlazorWasm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
                var result = await authService.Authorize(model);
                Token = result.Token;
                var claims = ParseClaimsFromJwt(Token);

                foreach (var claim in claims)
                {
                    Console.WriteLine(claim);
                }

                await localStorageService.SetItem("token", Token);
            }
            catch (Exception ex)
            {
                Token = null;
                await localStorageService.RemoveItem("token");
            }

            return IsAuthenticated;
        }

        private IList<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())).ToList();
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
