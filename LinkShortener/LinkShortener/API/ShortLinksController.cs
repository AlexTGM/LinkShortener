using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Models.Database;
using LinkShortener.API.Models.DTO;
using LinkShortener.API.Services.LinkShortener;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.API
{
    [Produces("application/json")]
    [Route("api/shortened")]
    public class ShortLinksController : Controller
    {
        private readonly ILinkShortenerService _service;
        private readonly UserManager<User> _userManager;

        public ShortLinksController(ILinkShortenerService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        private Task<User> CurrentUser => _userManager.GetUserAsync(User);

        [HttpGet]
        [Authorize]
        public async Task<PaginatedData<ShortLinkDto>> Get(int skip, int take)
        {
            return await _service.GetAllShortenedLinksRelatedToUserPaginatedAsync(await CurrentUser, skip, take);
        }

        [HttpGet("{shortLink}", Name = "Get")]
        public IActionResult Get(string shortLink)
        {
            return Redirect(_service.GetFullLinkAsync(shortLink).Result.FullLink);
        }

        [HttpPost]
        public async Task<dynamic> Post([FromBody] string fullLink)
        {
            return new {ShortLink = await _service.CreateShortLinkAsync(fullLink, await CurrentUser)};
        }
    }
}