using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class TagData
    {
        public TagData()
        {
            PostTags = new HashSet<PostTag>();
        }

        public long Id { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
