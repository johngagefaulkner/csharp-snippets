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
    
    // Uses the provided HttpClient to send a GET request to the provided URL.
    // Assumes the response is a JSON string and converts it to the provided type.
    public static async Task<T> GetFromUrlAsync<T>(string jsonUrl, HttpClient _client)
    {
        return await _client.GetFromJsonAsync<T>(jsonUrl);
    }
  }
}
