using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LinkShortener.API.Models
{
    public class SigninModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class User : IdentityUser
    {
        public User() { }

        public User(string userName) : base(userName) { }
    }
}