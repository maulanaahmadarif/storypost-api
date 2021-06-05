using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class BanUserDto
    {
        public long id { get; set; }
    }

    public partial class ChangePasswordDto
    {
        public long id { get; set; }
        public string password { get; set; }
    }

    public partial class UpdateUserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Nip { get; set; }
        public string Account { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? RoleId { get; set; }
    }

    public partial class UserActivityDto {
        public string Username { get; set; }
        public string LoginDate { get; set; }
    }
}