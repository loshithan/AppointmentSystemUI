using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace AppointmentUI.Components.Services
{
    public class JMTAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AccessTokenService _accessTokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        // Constructor to inject AccessTokenService
        public JMTAuthenticationStateProvider(AccessTokenService accessTokenService, IHttpClientFactory httpClientFactory)
        {
            _accessTokenService = accessTokenService;
            _httpClientFactory = httpClientFactory;
        }

        // Override method to get the authentication state
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                // Retrieve the token from the AccessTokenService
                var token = await _accessTokenService.GetTokenAsync();

                // If the token is null or empty, mark the user as unauthorized
                if (string.IsNullOrWhiteSpace(token))
                {
                    return await MarkAsUnauthorizedAsync();
                }

                // Read and validate the JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                // Create a ClaimsIdentity from the token's claims
                var identity = new ClaimsIdentity(jwtToken.Claims, "JWT");

                // Create a ClaimsPrincipal from the ClaimsIdentity
                var principal = new ClaimsPrincipal(identity);

                var _httpClient = _httpClientFactory.CreateClient("ApiClient");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);


                // Return the AuthenticationState with the authenticated principal
                return await Task.FromResult(new AuthenticationState(principal));
            }
            catch (Exception ex)
            {
                // If an exception occurs, mark the user as unauthorized
                return await MarkAsUnauthorizedAsync();
            }
        }

        // Method to mark the user as unauthorized
        private async Task<AuthenticationState> MarkAsUnauthorizedAsync()
        {
            // Create an empty ClaimsPrincipal (unauthorized)
            var anonymousIdentity = new ClaimsIdentity();
            var anonymousPrincipal = new ClaimsPrincipal(anonymousIdentity);

            // Notify the authentication state has changed
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousPrincipal)));

            return new AuthenticationState(anonymousPrincipal);
        }
    }
}