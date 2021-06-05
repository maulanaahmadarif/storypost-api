using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class Notification
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? PostDataId { get; set; }
        public long? FromId { get; set; }
        public int? Type { get; set; }
        public string Thumbnail { get; set; }
        public int? Viewed { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual PostData PostData { get; set; }
        public virtual User User { get; set; }
    }
}
