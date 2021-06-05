using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class UserRole
    {
        public UserRole()
        {
            ListMenus = new HashSet<ListMenu>();
            Users = new HashSet<User>();
        }

        public long Id { get; set; }
        public string RoleName { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<ListMenu> ListMenus { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
