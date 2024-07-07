using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Snippets.Services.HTTP
{
    public class HttpService
    {
        private static readonly HttpClient _httpClient = new();

        /// <summary>
        /// Provides a way for the internal/private HttpClient object to be accessed directly if necessary.
        /// </summary>
        public static HttpClient GetHttpClient() => _httpClient;

        /// <summary>
        /// Performs an HTTP GET request to a URL using an HTTP Authorization header.
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public static async Task<string> GetStringFromAuthenticatedRequestAsync(string url, string token)
        {
            HttpResponseMessage response;
            try
            {
                // Create the HttpRequestMessage to send
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                
                // Add the token in Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Send the request and ensure the response's status code indicates success
                response = await _httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode())
                {
                    return await response.Content.ReadAsStringAsync();
                }

                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // Uses the provided HttpClient to send a GET request to the provided URL.
        // Assumes the response is a JSON string and converts it to the provided type.
        public static async Task<T> GetFromUrlAsync<T>(string httpRequestUrl) => await _httpClient.GetFromJsonAsync<T>(httpRequestUrl);

        public static async Task<bool> DownloadFileAsync(string _url, string _filename)
        {
            try
            {
                using (WebClient wc = new())
                {
                    await wc.DownloadFileTaskAsync(_url, _filename);
                }

                Task.WaitAll();
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to download file with error message: {ex.Message}");
                return false;
            }
        }
    }
}
