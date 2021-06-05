using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class PostImage
    {
        public long Id { get; set; }
        public long? PostDataId { get; set; }
        public string Path { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? Type { get; set; }

        public virtual PostData PostData { get; set; }
    }
}
