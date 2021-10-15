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
        public static HttpClient GlobalHttpClient { get; set; } = null;

        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public async Task<string> GetHttpContentWithToken(string url, string token, HttpClient _client)
        {
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                response = await _client.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        // Uses the provided HttpClient to send a GET request to the provided URL.
        // Assumes the response is a JSON string and converts it to the provided type.
        public static async Task<T> GetFromUrlAsync<T>(string jsonUrl, HttpClient _client)
        {
            return await _client.GetFromJsonAsync<T>(jsonUrl);
        }

        #region WorkingWithFiles
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
                string errorStr = "Error: " + ex.Message;
                return false;
            }
        }

        public static async Task<string> DownloadRSSFeed(string _url, string _filename)
        {
            try
            {
                using (WebClient wc = new())
                {
                    var _result = await wc.DownloadStringTaskAsync(_url);
                    await File.WriteAllTextAsync(_filename, _result.Trim());
                }

                string str = await File.ReadAllTextAsync(_filename);
                Task.WaitAll();
                return str.Trim();
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        #endregion
    }
}
