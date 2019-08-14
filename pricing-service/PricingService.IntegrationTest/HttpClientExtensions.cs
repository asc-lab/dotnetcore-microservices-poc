using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PricingService.IntegrationTest
{
    static class HttpClientExtensions
    {
        public static async Task<RestResult<T>> DoPostAsync<T>(this HttpClient client, string uri, object data)
            where T : class
        {
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(uri, requestContent).ConfigureAwait(false);
            
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var result = JsonConvert.DeserializeObject<T>(responseContent);
                return RestResult<T>.Ok(result);
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorDetails = JsonConvert.DeserializeAnonymousType(responseContent, new { Code = "", Message = "" });
                return RestResult<T>.Error(errorDetails.Code, errorDetails.Message);
            }
            
        }


        public static async Task<T> DoGetAsync<T>(this HttpClient client, string uri)
            where T : class
        {
            var response = await client.GetAsync(uri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<T>(responseContent);

            return result;
        }
    }

    public class RestResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }

        public static RestResult<T> Ok(T data)
        {
            return new RestResult<T>
            {
                Data = data,
                Success = true
            };
        }

        public static RestResult<T> Error(string code, string message)
        {
            return new RestResult<T>
            {
                Success = false,
                ErrorCode = code,
                ErrorMessage = message
            };
        }
    }
}
