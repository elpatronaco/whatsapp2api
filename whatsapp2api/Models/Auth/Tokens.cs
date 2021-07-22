namespace whatsapp2api.Models.Auth
{
    public class Tokens
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
    }

    public class AccessTokenPayload
    {
        public string Phone { get; set; }
        public string Username { get; set; }
    }

    public class IdTokenPayload
    {
        public string Id { get; set; }
    }

    public class RefreshTokenPayload
    {
        public string Id { get; set; }
        public string Hash { get; set; }
    }
}