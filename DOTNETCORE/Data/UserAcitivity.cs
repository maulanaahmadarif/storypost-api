using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class UserAcitivity
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public DateTime? LastLogin { get; set; }

        public virtual User User { get; set; }
    }
}
