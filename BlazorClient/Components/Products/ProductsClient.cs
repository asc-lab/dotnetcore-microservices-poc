using System.Net.Http.Headers;
using BlazorClient.Components.Auth;
using BlazorClient.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using ProductService.Api.Queries.Dtos;

namespace BlazorClient.Components.Products;

public class ProductsClient
{
    private readonly CustomAuthenticationStateProvider customAuthenticationStateProvider;
    private readonly HttpClient httpClient;
    private readonly string baseUrl;

    public ProductsClient
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

    public async Task<ApiResponse<IEnumerable<ProductDto>>> GetAllProducts()
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var productsString = await httpClient
            .GetStringAsync($"{baseUrl}/api/Products");

        return ApiResponse<IEnumerable<ProductDto>>.Success
        (
            JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(productsString)
        );
    }
    
    public async Task<ApiResponse<ProductDto>> GetProduct(string code)
    {
        var token = await customAuthenticationStateProvider.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var productsString = await httpClient
            .GetStringAsync($"{baseUrl}/api/Products/{code}");

        return ApiResponse<ProductDto>.Success
        (
            JsonConvert.DeserializeObject<ProductDto>(productsString)
        );
    }
}