namespace geckserver.Configuration.DTO
{
    public partial class LoginDTO
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public partial class GetTokenDto
    {
        public string account { get; set; }
    }
}