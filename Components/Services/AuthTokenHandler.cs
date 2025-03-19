using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AppointmentUI.Components.Services
{
    public class AuthTokenHandler : DelegatingHandler
{
    private readonly AccessTokenService _accessTokenService;

    public AuthTokenHandler(AccessTokenService accessTokenService)
    {
        _accessTokenService = accessTokenService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Get the token from the AccessTokenService
        var token = await _accessTokenService.GetTokenAsync();

        // Add the token to the Authorization header
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        // Proceed with the request
        return await base.SendAsync(request, cancellationToken);
    }
}

}