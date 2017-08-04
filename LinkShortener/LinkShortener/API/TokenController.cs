using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using AspNet.Security.OpenIdConnect.Server;
using LinkShortener.API.Models.Database;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Core;

namespace LinkShortener.API
{
    [Route("connect/token")]
    public class TokenController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public TokenController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Token(OpenIdConnectRequest request)
        {
            if (!request.IsPasswordGrantType())
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.UnsupportedGrantType,
                    ErrorDescription = "The specified grant type is not supported."
                });
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "Invalid username or password"
                });
            }
            
            if (!await _signInManager.CanSignInAsync(user) || (_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user)))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "The specified user cannot sign in."
                });
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return BadRequest(new OpenIdConnectResponse
                {
                    Error = OpenIdConnectConstants.Errors.InvalidGrant,
                    ErrorDescription = "Invalid username or password"
                });
            }
            
            if (_userManager.SupportsUserLockout)
            {
                await _userManager.ResetAccessFailedCountAsync(user);
            }
            
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            
            foreach (var claim in principal.Claims)
            {
                claim.SetDestinations(OpenIdConnectConstants.Destinations.AccessToken, OpenIdConnectConstants.Destinations.IdentityToken);
            }
            
            var ticket = new AuthenticationTicket(
                principal,
                new AuthenticationProperties(),
                OpenIdConnectServerDefaults.AuthenticationScheme);
            
            var scope = new[]
            {
                OpenIdConnectConstants.Scopes.OpenId,
                OpenIdConnectConstants.Scopes.Email,
                OpenIdConnectConstants.Scopes.Profile,
                OpenIdConnectConstants.Scopes.OfflineAccess,
                OpenIddictConstants.Scopes.Roles
            }.Intersect(request.GetScopes());

            ticket.SetResources("http://localhost:50110/");
            ticket.SetScopes(scope);
            
            return SignIn(ticket.Principal, ticket.Properties, ticket.AuthenticationScheme);
        }
    }
}
