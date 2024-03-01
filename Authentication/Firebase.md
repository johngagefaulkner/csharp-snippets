# Authentication / Firebase Auth

![firebaseAuthImg](https://firebase.google.com/static/docs/auth/images/auth-providers.png)

## Overview

### What is it?
User sign-up and sign-in using Firebase Authentication's REST API ─ perfectly suited to run on clients (end-user devices, like your Desktop PC running Windows 11, for example) as the only thing that gets exposed is the Web API Key for your Firebase Project (which is already a public-facing API key, anyway.)

### March 1, 2024
Someone reached out to me via Discord asking for help signing users into their C# console application using Firebase Authentication. Immediately, I wanted to tell them it'll never work because Google refuses to create ***CLIENT*** SDKs for C# (.NET). However, after giving it some thought, I remembered that the Firebase Authentication service exposed a REST API that only required the public-facing "Web API Key" from the project you wanted to authenticate against. While it seems that the breadcrumb navigation has been removed from the Firebase Auth docs, ChatGPT 4 had no problems finding the link! For those interested, here's the source docs: 

## Code
For those of you who simply want to see the implementation, he's a fully working class that can be copied and pasted into nearly any C# / .NET project:

```cs
public static class FirebaseAuthService
{
  // Replace with your "Web API Key" (from "https://console.firebase.google.com/project/{YOUR_PROJECT_ID}/settings/general/")
  private const string apiKey = "";
  
  // Build the REST API request URL
  private static string firebaseAuthSignInUrl = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";
  
  // Properly-handled single-instance HttpClient
  private static readonly HttpClient _instanceHttpClient = new();
  
  // Use the new "Record" class to easily create the user authentication credentials model in a single line.
  public record FirebaseUserLoginData(string email, string password, bool returnSecureToken);
  
  // Attempt to sign in the user using the provided credentials
  public static async Task<string> ProcessUserSignInAsync(string userEmail, string userPass)
  {
    try
    {
      // Create a JSON payload
      var _userLoginData = new FirebaseUserLoginData(userEmail, userPass, true);
      var _userLoginDataJSON = JsonSerializer.Serialize(_userLoginData);
  
      // Clear the HttpClient's Default Request Headers
      _instanceHttpClient.DefaultRequestHeaders.Clear();
  
      // Send a POST request containing the user's credentails to attempt login
      var response = await _instanceHttpClient.PostAsync(firebaseAuthSignInUrl, new StringContent(_userLoginDataJSON, Encoding.UTF8, "application/json"));
  
      // Read the response content
      return await response.Content.ReadAsStringAsync();
    }
  
    catch (Exception ex)
    {
      var errorMsg = $"[ERROR] Failed to process user sign-in: {ex.Message}";
      return errorMsg;
    }
  }
}
```

