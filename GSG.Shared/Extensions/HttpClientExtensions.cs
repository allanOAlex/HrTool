using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Shared.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync<TValue>(this HttpClient httpClient, string requestUri, TValue value)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Content = JsonContent.Create(value),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri, UriKind.Relative)
            };
            return await httpClient.SendAsync(request);
        }

        //public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data)
        //=> httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, string requestUri, T data, CancellationToken cancellationToken)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) });

        public static Task<HttpResponseMessage> DeleteAsJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T data, CancellationToken cancellationToken)
            => httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = Serialize(data) }, cancellationToken);

        private static HttpContent Serialize(object data) => new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    }
}
