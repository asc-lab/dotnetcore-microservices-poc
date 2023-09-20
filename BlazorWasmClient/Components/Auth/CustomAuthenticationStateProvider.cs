using System.Security.Claims;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWasmClient.Components.Auth;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ISessionStorageService sessionStorageService;
    private ILogger<CustomAuthenticationStateProvider> logger;
    private ClaimsPrincipal anonymous = new ClaimsPrincipal(new ClaimsIdentity[]{});

    public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService, ILogger<CustomAuthenticationStateProvider> logger)
    {
        this.sessionStorageService = sessionStorageService;
        this.logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSession = await sessionStorageService.ReadEncryptedItem<UserSession>("UserSession");
            if (userSession == null)
                return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.UserName),
                new Claim(ClaimTypes.Role, userSession.Role),
            }, "JwtAuth"));
            
            return await Task.FromResult(new AuthenticationState(claimsPrinciple));
        }
        catch (Exception ex)
        {
            logger.LogError("Exception in auth state provider", ex);
            return await Task.FromResult(new AuthenticationState(anonymous));
        }
    }

    public async Task UpdateAuthenticationState(UserSession userSession)
    {
        ClaimsPrincipal claimsPrincipal = anonymous;

        if (userSession != null)
        {
            var claimsPrinciple = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Name, userSession.UserName),
                new Claim(ClaimTypes.Role, userSession.Role),
            }));
            //userSession.ExpiryTimestamp = DateTime.Now.AddSeconds(userSession.ExpiresIn);
            await sessionStorageService.SaveItemAsEncrypted("UserSession", userSession);
        }
        else
        {
            await sessionStorageService.RemoveItemAsync("UserSession");
        }
        
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task<string> GetToken()
    {
        var result = string.Empty;

        var userSession = await sessionStorageService.ReadEncryptedItem<UserSession>("UserSession");
        //TODO: handle expiry
        //if (userSession!=null && DateTime.Now < userSession.ExpiryTimeStamp)
        result = userSession.Token;
        
        return result;
    }
    
}