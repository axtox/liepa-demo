using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using LiepaService.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LiepaService.Handlers {

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ILiepaAuthenticationService _authenticationService;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ILiepaAuthenticationService authenticationService)
            : base(options, logger, encoder, clock)
        {
            _authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(HasAllowAnonymousAttribute())
                return AuthenticateResult.NoResult(); //skip

            
            if (!HasBasicAuthorizationHeader())
                return AuthenticateResult.Fail("Provide 'Authorization' header in your request.");

            NetworkCredential credential;
            if(!RetrieveCredential(out credential))
                return AuthenticateResult.Fail("Incorrect Authorization Header.");

            var result = await _authenticationService.Authenticate(credential);
            if (!result)
                return AuthenticateResult.Fail("Incorrect Username or Password. Try again.");

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, credential.UserName)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private bool HasAllowAnonymousAttribute() {
            var endpoint = Context.GetEndpoint();

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
                return true;
            
            return false;
        }

        private bool HasBasicAuthorizationHeader() {
            if(!Request.Headers.ContainsKey("Authorization"))
                return false;

            if(!Request.Headers["Authorization"].FirstOrDefault()?.Contains("Basic") ?? false)
                return false;

            return true;
        }

        private bool RetrieveCredential(out NetworkCredential credential) {
            string[] credentials;
            try {
                var authorizationHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authorizationHeader.Parameter);
                credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
            }
            catch {
                credential = null;
                return false;
            }

            var username = credentials[0];
            var password = credentials[1];
            credential = new NetworkCredential(username, password);
            return true;
        }
    }
}