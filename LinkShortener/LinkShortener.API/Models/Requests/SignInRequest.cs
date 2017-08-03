namespace LinkShortener.API.Models.Requests
{
    public class SignInRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }

    public class SignUpRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}