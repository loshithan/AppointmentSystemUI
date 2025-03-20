using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace AppointmentUI.Components.Services
{
    public class UserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string?> GetUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                // Replace "nameid" with the actual claim type for UserId in your token
                return user.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            }

            return null; // User is not authenticated
        }

        public async Task<string?> GetUserNameAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                // Replace "nameid" with the actual claim type for UserId in your token
                return user.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
            }

            return null; // User is not authenticated
        }

        public async Task<string?> GetClaimValueAsync(string claimType)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                return user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }

            return null; // User is not authenticated
        }
    }
}