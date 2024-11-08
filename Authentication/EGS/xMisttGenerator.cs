using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

class EpicUser
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
    [JsonPropertyName("expires_at")]
    public string ExpiresAt { get; set; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; }
    [JsonPropertyName("refresh_expires")]
    public string RefreshExpires { get; set; }
    [JsonPropertyName("refresh_expires_at")]
    public string RefreshExpiresAt { get; set; }
    [JsonPropertyName("account_id")]
    public string AccountId { get; set; }
    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }
    [JsonPropertyName("internal_client")]
    public bool InternalClient { get; set; }
    [JsonPropertyName("client_service")]
    public string ClientService { get; set; }
    [JsonPropertyName("displayName")]
    public string DisplayName { get; set; }
    [JsonPropertyName("app")]
    public string App { get; set; }
    [JsonPropertyName("in_app_id")]
    public string InAppId { get; set; }

    public static async Task<string> GetEmailAsync(HttpClient httpClient, string displayName, string accessToken)
    {
        var response = await httpClient.GetAsync($"https://account-public-service-prod03.ol.epicgames.com/account/api/public/account/displayName/{displayName}");
        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(content);

        return data["email"];
    }
}

class EpicGenerator
{
    private static readonly string SwitchToken = "OThmN2U0MmMyZTNhNGY4NmE3NGViNDNmYmI0MWVkMzk6MGEyNDQ5YTItMDAxYS00NTFlLWFmZWMtM2U4MTI5MDFjNGQ3";
    private static readonly string IosToken = "MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=";
    private static readonly string Version = "1.1.1";
    private HttpClient _httpClient;
    private string _accessToken;

    public EpicGenerator()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", $"DeviceAuthGenerator/{Version} {Environment.OSVersion}");
    }

    public async Task StartAsync()
    {
        _accessToken = await GetAccessTokenAsync();

        Console.WriteLine($"DeviceAuthGenerator v{Version} made by xMistt // Oli. Ask for help in PartyBot.");
        await Task.Delay(3000);
        Console.Clear();

        while (true)
        {
            Console.WriteLine("Opening device code link in a new tab.");

            var deviceCode = await CreateDeviceCodeAsync();
            Process.Start(new ProcessStartInfo($"https://www.epicgames.com/activate?userCode={deviceCode.Item1}") { UseShellExecute = true });

            var user = await WaitForDeviceCodeCompletionAsync(deviceCode.Item2);
            var deviceAuths = await CreateDeviceAuthsAsync(user);

            Console.Clear();
            var prettyDeviceAuths = JsonSerializer.Serialize(deviceAuths, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"Generated device auths for: {user.DisplayName}.");
            Console.WriteLine(prettyDeviceAuths);

            // Copy to clipboard
            TextCopy.ClipboardService.SetText(prettyDeviceAuths);
            await SaveDeviceAuthsAsync(deviceAuths, user);

            Console.WriteLine("\nThey have been copied to the clipboard & saved in device_auths.json.");
            Console.Write("Do you want to generate another device auth? (Y/N): ");
            var choice = Console.ReadLine();

            if (choice.Trim().ToLower() != "y")
            {
                break;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Closing DeviceAuthGenerator...");
                await Task.Delay(1000);
            }
        }

        _httpClient.Dispose();
        Environment.Exit(0);
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var request = new HttpRequestMessage(HttpMethod.Post, "https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token")
        {
            Content = requestContent
        };
        request.Headers.Add("Authorization", $"basic {SwitchToken}");
        request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(content);

        return data["access_token"];
    }

    private async Task<(string, string)> CreateDeviceCodeAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/deviceAuthorization")
        {
            Headers = { { "Authorization", $"bearer {_accessToken}" }, { "Content-Type", "application/x-www-form-urlencoded" } }
        };

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(content);

        return (data["user_code"], data["device_code"]);
    }

    private async Task<EpicUser> WaitForDeviceCodeCompletionAsync(string deviceCode)
    {
        Console.Clear();
        Console.WriteLine("Waiting for completion.");

        while (true)
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "device_code"),
                new KeyValuePair<string, string>("device_code", deviceCode)
            });

            var request = new HttpRequestMessage(HttpMethod.Post, "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token")
            {
                Content = requestContent
            };
            request.Headers.Add("Authorization", $"basic {SwitchToken}");
            request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var response = await _httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var token = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
                var exchangeRequest = new HttpRequestMessage(HttpMethod.Get, "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/exchange")
                {
                    Headers = { { "Authorization", $"bearer {token["access_token"]}" } }
                };

                var exchangeResponse = await _httpClient.SendAsync(exchangeRequest);
                var exchangeContent = await exchangeResponse.Content.ReadAsStringAsync();
                var exchange = JsonSerializer.Deserialize<Dictionary<string, string>>(exchangeContent);

                var authRequestContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "exchange_code"),
                    new KeyValuePair<string, string>("exchange_code", exchange["code"])
                });

                var authRequest = new HttpRequestMessage(HttpMethod.Post, "https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token")
                {
                    Content = authRequestContent
                };
                authRequest.Headers.Add("Authorization", $"basic {IosToken}");
                authRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var authResponse = await _httpClient.SendAsync(authRequest);
                var authContent = await authResponse.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<EpicUser>(authContent);
            }

            await Task.Delay(5000);
        }
    }

    private async Task<Dictionary<string, object>> CreateDeviceAuthsAsync(EpicUser user)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://account-public-service-prod.ol.epicgames.com/account/api/public/account/{user.AccountId}/deviceAuth")
        {
            Headers = { { "Authorization", $"bearer {user.AccessToken}" }, { "Content-Type", "application/json" } }
        };

        var response = await _httpClient.SendAsync(request);
        var content = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<Dictionary<string, object>>(content);

        return new Dictionary<string, object>
        {
            { "device_id", data["deviceId"] },
            { "account_id", data["accountId"] },
            { "secret", data["secret"] },
            { "user_agent", data["userAgent"] },
            { "created", new Dictionary<string, object>
                {
                    { "location", data["created"]["location"] },
                    { "ip_address", data["created"]["ipAddress"] },
                    { "datetime", data["created"]["dateTime"] }
                }
            }
        };
    }

    private async Task SaveDeviceAuthsAsync(Dictionary<string, object> deviceAuths, EpicUser user)
    {
        Dictionary<string, Dictionary<string, object>> current;

        if (File.Exists("device_auths.json"))
        {
            var json = await File.ReadAllTextAsync("device_auths.json");
            current = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(json);
        }
        else
        {
            current = new Dictionary<string, Dictionary<string, object>>();
        }

        var email = await EpicUser.GetEmailAsync(_httpClient, user.DisplayName, user.AccessToken);
        current[email] = deviceAuths;

        var updatedJson = JsonSerializer.Serialize(current, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync("device_auths.json", updatedJson);
    }

    public static async Task Main(string[] args)
    {
        var generator = new EpicGenerator();
        await generator.StartAsync();
    }
}