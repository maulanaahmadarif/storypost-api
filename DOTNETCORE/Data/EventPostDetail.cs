using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class EventPostDetail
    {
        public long Id { get; set; }
        public long? EventPostId { get; set; }
        public long? PostDataId { get; set; }

        public virtual EventPost EventPost { get; set; }
        public virtual PostData PostData { get; set; }
    }
}
