using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class UserProfileDTO
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Nip { get; set; }
        public string RoleId { get; set; }
    }

    public partial class UserProfileUpdateDto {
        [Required]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
    }

    public partial class ProfilePictureDto {
        public IFormFile file { get; set; }
    }

    public partial class StoryPostDto {
        public List<IFormFile> files { get; set; }
    }
}