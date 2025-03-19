using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppointmentUI.Components.Services
{
     public class AccessTokenService
    {
        private readonly CookieService _cookieService;
        private readonly string _tokenKey = "access_token";

        // Constructor to inject CookieService
        public AccessTokenService(CookieService cookieService)
        {
            _cookieService = cookieService;
        }

        // Method to set the access token in cookies
        public async Task SetTokenAsync(string accessToken)
        {
            await _cookieService.SetCookieAsync(_tokenKey, accessToken, 1); // 1 day expiration
        }

        // Method to get the access token from cookies
        public async Task<string> GetTokenAsync()
        {
            return await _cookieService.GetCookieAsync(_tokenKey);
        }

        // Method to remove the access token from cookies
        public async Task RemoveTokenAsync()
        {
            await _cookieService.DeleteCookieAsync(_tokenKey);
        }
    }
}