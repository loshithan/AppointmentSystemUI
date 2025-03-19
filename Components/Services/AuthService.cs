using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AppointmentUI.Components.Services;
using BlazorServer.Services;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace BlazorServer.Services
{
    public class AuthService
    {
        private readonly AccessTokenService _accessTokenService;
        private readonly NavigationManager _nav;
        private readonly HttpClient _client;

        // Constructor to inject dependencies
        public AuthService(
            AccessTokenService accessTokenService,
            NavigationManager nav,
            IHttpClientFactory httpClientFactory)
        {
            _accessTokenService = accessTokenService;
            _nav = nav;
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // Method to handle user login
        public async Task<bool> LoginAsync(string username, string password)
        {
            // Send login request to the API
            var response = await _client.PostAsJsonAsync("Auth/login", new { username, password });

            if (response.IsSuccessStatusCode)
            {
                // Read the response content
                var token = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<AuthResponse>(token);

                // Store the access token
                await _accessTokenService.SetTokenAsync(result.Token);

                // Navigate to a protected page or home page
                _nav.NavigateTo("/");

                return true;
            }
            else
            {
                // Login failed
                return false;
            }
        }

        // Method to handle user logout
        public async Task LogoutAsync()
        {
            // Remove the access token
            await _accessTokenService.RemoveTokenAsync();

            // Navigate to the login page
            _nav.NavigateTo("/login");
        }

        // Method to check if the user is authenticated
        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await _accessTokenService.GetTokenAsync();
            return !string.IsNullOrEmpty(token);
        }
    }

    // Class to represent the authentication response from the API
    public class AuthResponse
    {
        public string Token { get; set; }
    }
}