using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LinkShortener.API.Models.Database
{
    public class User : IdentityUser
    {
        public User()
        {
        }

        public User(string userName) : base(userName)
        {
        }
    }
}