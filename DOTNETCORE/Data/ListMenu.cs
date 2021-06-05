using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class ListMenu
    {
        public long Id { get; set; }
        public string MenuName { get; set; }
        public long? UserRoleId { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual UserRole UserRole { get; set; }
    }
}
