using System.Net.Http.Headers;
using BlazorClient.Components.Auth;
using BlazorClient.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using PolicySearchService.Api.Queries;

namespace BlazorClient.Components.PolicySearch;

public class PolicySearchClient
{
    private readonly CustomAuthenticationStateProvider customAuthenticationStateProvider;
    private readonly HttpClient httpClient;
    private readonly string baseUrl;
    private const string And = "%20AND%20";

    public PolicySearchClient
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

    public async Task<ApiResponse<FindPolicyResult>> SearchPolicies(string policyNumber, string policyHolder)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var queryString = string.Empty;
        if (!string.IsNullOrWhiteSpace(policyNumber))
        {
            queryString += policyNumber;
        }

        if (!string.IsNullOrWhiteSpace(policyHolder))
        {
            queryString += ((queryString.Length>0 ? And : string.Empty) + policyHolder);
        }
        
        var productsString = await httpClient
            .GetStringAsync($"{baseUrl}/api/PolicySearch?q={queryString}");

        return ApiResponse<FindPolicyResult>.Success(JsonConvert.DeserializeObject<FindPolicyResult>(productsString));
    }
}