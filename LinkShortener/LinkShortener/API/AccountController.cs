using System.Threading.Tasks;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.API
{
    [Route("api/account")]
    [Produces("application/json")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;

        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] SignUpRequest signUpRequest)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = signUpRequest.UserName, Email = signUpRequest.UserName };
                var result = await _userManager.CreateAsync(user, signUpRequest.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }

                AddErrors(result);
            }

            return BadRequest(ModelState);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}