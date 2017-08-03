using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.API.Models;
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

        private Task<User> CurrentUser => _userManager.GetUserAsync(User);

        public ShortLinksController(ILinkShortenerService service, UserManager<User> userManager)
        {
            _service = service;
            _userManager = userManager;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<ShortLink>> Get()
            => await _service.GetAllShortenedLinksRelatedToUserAsync(await CurrentUser);
        
        [HttpGet("{value}", Name = "Get")]
        public async Task<ShortLink> Get(string value)
            => await _service.GetFullLinkAsync(value);
        
        [HttpPost]
        [Authorize]
        public async Task<dynamic> Post([FromBody] string value)
            => new {ShortLink = await _service.CreateShortLinkAsync(value, await CurrentUser)};
    }
}