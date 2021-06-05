using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class PostTag
    {
        public long Id { get; set; }
        public long? PostDataId { get; set; }
        public long TagDataId { get; set; }

        public virtual PostData PostData { get; set; }
        public virtual TagData TagData { get; set; }
    }
}
