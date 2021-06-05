using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class PostReport
    {
        public long Id { get; set; }
        public long? PostDataId { get; set; }
        public long? UserId { get; set; }
        public string Reason { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string StatusDesc { get; set; }
        public int? IsDecline { get; set; }
        public int? IsRemove { get; set; }
        public int? IsEmailed { get; set; }

        public virtual PostData PostData { get; set; }
        public virtual User User { get; set; }
    }
}
