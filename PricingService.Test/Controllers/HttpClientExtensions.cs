using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PricingService.Test.Controllers
{
    static class HttpClientExtensions
    {
        public static async Task<T> DoPostAsync<T>(this HttpClient client, string uri, Object data)
            where T : class
        {
            var requestContent = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync(uri, requestContent).ConfigureAwait(false);
            //response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonConvert.DeserializeObject<T>(responseContent);

            return result;
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
}
