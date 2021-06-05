using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class User
    {
        public User()
        {
            Notifications = new HashSet<Notification>();
            PostData = new HashSet<PostData>();
            PostLikes = new HashSet<PostLike>();
            PostReports = new HashSet<PostReport>();
            UserAcitivities = new HashSet<UserAcitivity>();
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
        public string Nip { get; set; }
        public string Account { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? Confirmed { get; set; }
        public string Secretcode { get; set; }
        public int? IsBanned { get; set; }
        public int? IsDownload { get; set; }
        public int? IsEmailBanned { get; set; }
        public long? RoleId { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<PostData> PostData { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<PostReport> PostReports { get; set; }
        public virtual ICollection<UserAcitivity> UserAcitivities { get; set; }
    }
}
