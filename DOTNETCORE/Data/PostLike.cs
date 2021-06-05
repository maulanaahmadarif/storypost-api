using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class PostLike
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? PostDataId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual PostData PostData { get; set; }
        public virtual User User { get; set; }
    }
}
