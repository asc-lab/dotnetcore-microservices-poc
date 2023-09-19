using System.Net.Http.Headers;
using System.Text;
using BlazorClient.Components.Auth;
using BlazorClient.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using PolicyService.Api.Commands;
using PolicyService.Api.Queries;

namespace BlazorClient.Components.Products;

public class PolicyClient
{
    private readonly CustomAuthenticationStateProvider customAuthenticationStateProvider;
    private readonly HttpClient httpClient;
    private readonly string baseUrl;

    public PolicyClient
    (
        AuthenticationStateProvider customAuthenticationStateProvider, 
        HttpClient httpClient,
        IConfiguration configuration
    )
    {
        this.customAuthenticationStateProvider = (CustomAuthenticationStateProvider)customAuthenticationStateProvider;
        this.httpClient = httpClient;
        this.baseUrl = configuration["Services:ApiGateway"];
    }

    public async Task<ApiResponse<CreateOfferResult>> CreateOffer(CreateOfferCommand createOfferCommand)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var calculationResult = await httpClient.PostAsync
        (
            $"{baseUrl}/api/offers", 
            new StringContent(JsonConvert.SerializeObject(createOfferCommand), Encoding.UTF8, "application/json")
        );

        if (calculationResult.IsSuccessStatusCode)
        {
            var calculationResultJson = await calculationResult.Content.ReadAsStringAsync();
            return ApiResponse<CreateOfferResult>.Success(JsonConvert.DeserializeObject<CreateOfferResult>(calculationResultJson));
        }
        else
        {
            return ApiResponse<CreateOfferResult>.Error("Failed to calculate price.");
        }
    }
    
    public async Task<ApiResponse<CreatePolicyResult>> CreatePolicy(CreatePolicyCommand createPolicyCommand)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var calculationResult = await httpClient.PostAsync
        (
            $"{baseUrl}/api/policies", 
            new StringContent(JsonConvert.SerializeObject(createPolicyCommand), Encoding.UTF8, "application/json")
        );

        if (calculationResult.IsSuccessStatusCode)
        {
            var calculationResultJson = await calculationResult.Content.ReadAsStringAsync();
            return ApiResponse<CreatePolicyResult>.Success(JsonConvert.DeserializeObject<CreatePolicyResult>(calculationResultJson));
        }
        else
        {
            return ApiResponse<CreatePolicyResult>.Error("Failed to create policy.");
        }
    }

    public async Task<ApiResponse<GetPolicyDetailsQueryResult>> GetPolicy(string policyNumber)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var policy =
            await httpClient.GetFromJsonAsync<GetPolicyDetailsQueryResult>($"{baseUrl}/api/policies/{policyNumber}");

        return ApiResponse<GetPolicyDetailsQueryResult>.Success(policy);
    }
}