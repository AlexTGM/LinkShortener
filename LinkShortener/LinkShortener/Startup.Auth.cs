using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using LinkShortener.API.Models.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SimpleTokenProvider;

namespace LinkShortener
{
    public partial class Startup
    {
        private const string SecretKey = "mysupersecret_secretkey!123";

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        
        private void ConfigureAuth(IApplicationBuilder app)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            _userManager = app.ApplicationServices.GetRequiredService<UserManager<User>>();
            _signInManager = app.ApplicationServices.GetRequiredService<SignInManager<User>>();

            app.UseSimpleTokenProvider(new TokenProviderOptions
            {
                Path = "/api/token",
                Audience = "ExampleAudience",
                Issuer = "ExampleIssuer",
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                IdentityResolver = GetIdentity
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                
                ValidateIssuer = true,
                ValidIssuer = "ExampleIssuer",
                
                ValidateAudience = true,
                ValidAudience = "ExampleAudience",
                
                ValidateLifetime = true,
                
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = "Cookie",
                CookieName = "access_token",
                TicketDataFormat = new CustomJwtDataFormat(
                    SecurityAlgorithms.HmacSha256,
                    tokenValidationParameters)
            });
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, false, false);

            if (!result.Succeeded) return null;

            var user = await _userManager.FindByNameAsync(username);
            var claims = await _userManager.GetClaimsAsync(user);

            return (new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
        }
    }
}