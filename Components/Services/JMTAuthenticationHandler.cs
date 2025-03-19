using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace AppointmentUI.Components.Services
{
    public class JMTAuthenticationHandler : AuthenticationHandler<CustomOptions>
    {
        public JMTAuthenticationHandler(
            IOptionsMonitor<CustomOptions> options,
            ILoggerFactory logger,
            System.Text.Encodings.Web.UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                // Retrieve the token from the cookie
                var token = Request.Cookies["access_token"];
                if (string.IsNullOrEmpty(token))
                {
                    return AuthenticateResult.NoResult();
                }

                // Read and validate the JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                // Extract claims from the JWT
                var claims = jwtToken.Claims.ToList();
                var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                // Create a ClaimsIdentity from the token's claims
                var identity = new ClaimsIdentity(jwtToken.Claims, "JWT");

                // Create a ClaimsPrincipal from the ClaimsIdentity
                var principal = new ClaimsPrincipal(identity);

                // Create an AuthenticationTicket
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                // Return success with the ticket
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                // Log the exception and return failure
                Logger.LogError(ex, "Authentication failed.");
                return AuthenticateResult.Fail(ex);
            }
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Redirect to the login page
            Response.Redirect("/login");
            return Task.CompletedTask;
        }
        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            // Redirect to the login page
            Response.Redirect("/accessdenied");
            return Task.CompletedTask;
        }
    }

    public class CustomOptions : AuthenticationSchemeOptions
    {
    }
}