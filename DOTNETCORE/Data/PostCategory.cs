using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class PostCategory
    {
        public PostCategory()
        {
            EventPosts = new HashSet<EventPost>();
            PostData = new HashSet<PostData>();
        }

        public long Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public int IsReported { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Uid { get; set; }

        public virtual ICollection<EventPost> EventPosts { get; set; }
        public virtual ICollection<PostData> PostData { get; set; }
    }
}
