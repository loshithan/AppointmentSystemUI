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
                var token = await _accessTokenService.GetTokenAsync();

                if (string.IsNullOrWhiteSpace(token))
                {
                    return await MarkAsUnauthorizedAsync();
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                var claims = new List<Claim>();

                if (jwtToken != null)
                {
                    claims.AddRange(jwtToken.Claims);

                    // 🔹 Ensure multiple roles are handled correctly
                    var roleClaims = jwtToken.Claims.Where(c => c.Type == "role" || c.Type == ClaimTypes.Role).ToList();

                    foreach (var roleClaim in roleClaims)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value)); // Standardize role claims
                    }
                }

                var identity = new ClaimsIdentity(claims, "JWT");
                var principal = new ClaimsPrincipal(identity);

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));

                return new AuthenticationState(principal);
            }
            catch (Exception)
            {
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