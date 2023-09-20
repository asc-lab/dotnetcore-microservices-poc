namespace BlazorWasmClient.Components.Auth;

public class AuthResponse
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public string[] Roles { get; set; }
    public string Avatar { get; set; }
    public string UserType { get; set; }
    public DateTime ExpiresIn { get; set; }
}
