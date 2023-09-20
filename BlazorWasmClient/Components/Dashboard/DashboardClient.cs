using System.Net.Http.Headers;
using System.Text;
using BlazorWasmClient.Components.Auth;
using BlazorWasmClient.Shared;
using DashboardService.Api.Queries;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;

namespace BlazorWasmClient.Components.Dashboard;

public class DashboardClient
{
    private readonly CustomAuthenticationStateProvider customAuthenticationStateProvider;
    private readonly HttpClient httpClient;
    private readonly string baseUrl;
    
    public DashboardClient
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

    public async Task<ApiResponse<GetTotalSalesResult>> GetTotalSales(GetTotalSalesQuery query)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var calculationResult = await httpClient.PostAsync
        (
            $"{baseUrl}/api/Dashboard/total-sales", 
            new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")
        );

        if (calculationResult.IsSuccessStatusCode)
        {
            var calculationResultJson = await calculationResult.Content.ReadAsStringAsync();
            return ApiResponse<GetTotalSalesResult>.Success(JsonConvert.DeserializeObject<GetTotalSalesResult>(calculationResultJson));
        }
        else
        {
            return ApiResponse<GetTotalSalesResult>.Error("Failed to load data.");
        }
    }
    
    public async Task<ApiResponse<GetAgentsSalesResult>> GetAgentsSales(GetAgentsSalesQuery query)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var calculationResult = await httpClient.PostAsync
        (
            $"{baseUrl}/api/Dashboard/agents-sales", 
            new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")
        );

        if (calculationResult.IsSuccessStatusCode)
        {
            var calculationResultJson = await calculationResult.Content.ReadAsStringAsync();
            return ApiResponse<GetAgentsSalesResult>.Success(JsonConvert.DeserializeObject<GetAgentsSalesResult>(calculationResultJson));
        }
        else
        {
            return ApiResponse<GetAgentsSalesResult>.Error("Failed to load data.");
        }
    }
    
    public async Task<ApiResponse<GetSalesTrendsResult>> GetSalesTrends(GetSalesTrendsQuery query)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var calculationResult = await httpClient.PostAsync
        (
            $"{baseUrl}/api/Dashboard/sales-trends", 
            new StringContent(JsonConvert.SerializeObject(query), Encoding.UTF8, "application/json")
        );

        if (calculationResult.IsSuccessStatusCode)
        {
            var calculationResultJson = await calculationResult.Content.ReadAsStringAsync();
            return ApiResponse<GetSalesTrendsResult>.Success(JsonConvert.DeserializeObject<GetSalesTrendsResult>(calculationResultJson));
        }
        else
        {
            return ApiResponse<GetSalesTrendsResult>.Error("Failed to load data.");
        }
    }
}