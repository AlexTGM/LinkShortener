using System.Threading.Tasks;
using LinkShortener.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.API
{
    [Route("api/account")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IdentityResult> Signup([FromBody] SigninModel signinModel)
        {
            if (!ModelState.IsValid) return IdentityResult.Failed();

            var user = new User(signinModel.UserName);

            var identityResult = await _userManager.CreateAsync(user, signinModel.Password);

            if (!identityResult.Succeeded) return identityResult;

            await _signInManager.SignInAsync(user, true);

            return IdentityResult.Success;
        }

        [HttpGet]
        [Route("signout")]
        public async Task Signout()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpPost]
        [Route("signin")]
        public async Task<bool> Signin([FromBody] SigninModel signinModel)
        {
            var user = new User(signinModel.UserName);

            var result = await _signInManager.PasswordSignInAsync(user, signinModel.Password, true, false);

            return result.Succeeded;
        }
    }
}