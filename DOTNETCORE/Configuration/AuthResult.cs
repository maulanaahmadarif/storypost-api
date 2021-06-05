using System.Collections.Generic;
using geckserver.Configuration.DTO;
using geckserver.Data;

namespace geckserver.Configuration
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }

        public virtual UserProfileDTO Profile { get; set; }
        // public string Username { get; set; }
        // public string Email { get; set; }
        // public string NIP { get; set; }
    }
}