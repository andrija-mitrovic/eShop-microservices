using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebMVC.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public static Task<HttpResponseMessage> PostAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue(Constants.JSON_CONTENT_TYPE);

            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(this HttpClient httpClient, string url, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue(Constants.JSON_CONTENT_TYPE);

            return httpClient.PutAsync(url, content);
        }
    }
}