using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.API.Models;
using LinkShortener.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LinkShortener.API
{
    [Produces("application/json")]
    [Route("api/shortened")]
    public class ShortLinksController : Controller
    {
        private readonly ILinkShortenerService _service;

        public ShortLinksController(ILinkShortenerService service)
        {
            _service = service;
        }

        // GET: api/ShortLinks
        [HttpGet]
        public async Task<IEnumerable<ShortLink>> Get()
        {
            return await _service.GetAllShortenedLinksAsync();
        }

        // GET: api/ShortLinks/ABCDEF
        [HttpGet("{value}", Name = "Get")]
        public async Task<ShortLink> Get(string value)
        {
            return await _service.GetFullLinkAsync(value);
        }
        
        // POST: api/ShortLinks
        [HttpPost]
        public async Task<string> Post([FromBody]string value)
        {
            return await _service.CreateShortLinkAsync(value);
        }
    }
}