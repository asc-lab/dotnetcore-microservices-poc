using System.Net.Http.Json;
using BlazorWasmClient.Shared;

namespace BlazorWasmClient.Components.Auth;

public class AuthClient
{
    private readonly HttpClient httpClient;
    private readonly string baseUrl;

    public AuthClient
    (
        HttpClient httpClient,
        IConfiguration configuration
    )
    {
        this.httpClient = httpClient;
        this.baseUrl = configuration["Services:Auth"];
    }

    public async Task<ApiResponse<AuthResponse>> Login(AuthRequest loginRequest)
    {
        var loginResponse = await httpClient
            .PostAsJsonAsync<AuthRequest>($"{baseUrl}/api/User", loginRequest);

        if (loginResponse.IsSuccessStatusCode)
        {
            var authResponse = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();
            return ApiResponse<AuthResponse>.Success(authResponse);
        }
        else
        {
            return ApiResponse<AuthResponse>.Error("Failed to login. Please try again.");
        }
    }
}