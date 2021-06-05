using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class PostData
    {
        public PostData()
        {
            EventPostDetails = new HashSet<EventPostDetail>();
            Notifications = new HashSet<Notification>();
            PostImages = new HashSet<PostImage>();
            PostLikes = new HashSet<PostLike>();
            PostReports = new HashSet<PostReport>();
            PostTags = new HashSet<PostTag>();
        }

        public long Id { get; set; }
        public long UserId { get; set; }
        public long PostCategoryId { get; set; }
        public string Location { get; set; }
        public string Caption { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Uid { get; set; }
        public string Title { get; set; }
        public int? IsEvent { get; set; }
        public long? WeeklyId { get; set; }

        public virtual PostCategory PostCategory { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<EventPostDetail> EventPostDetails { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<PostImage> PostImages { get; set; }
        public virtual ICollection<PostLike> PostLikes { get; set; }
        public virtual ICollection<PostReport> PostReports { get; set; }
        public virtual ICollection<PostTag> PostTags { get; set; }
    }
}
