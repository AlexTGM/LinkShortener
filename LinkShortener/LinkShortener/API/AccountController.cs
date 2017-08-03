using System.Threading.Tasks;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Models.Requests;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.API
{
    [Route("api/account")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IdentityResult> SignUp([FromBody] SignUpRequest signUpRequest)
        {
            if (!ModelState.IsValid) return IdentityResult.Failed();

            var user = new User(signUpRequest.UserName);

            var identityResult = await _userManager.CreateAsync(user, signUpRequest.Password);

            if (!identityResult.Succeeded) return identityResult;

            await _signInManager.SignInAsync(user, true);

            return IdentityResult.Success;
        }

        [HttpGet]
        [Route("signout")]
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }

        [HttpPost]
        [Route("signin")]
        public async Task<bool> SignIn([FromBody] SignInRequest signInRequest)
        {
            var userName = signInRequest.UserName;
            var password = signInRequest.Password;
            var remember = signInRequest.Remember;

            var result = await _signInManager.PasswordSignInAsync(userName, password, remember, false);

            return result.Succeeded;
        }
    }
}