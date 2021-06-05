using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class EventPost
    {
        public EventPost()
        {
            EventPostDetails = new HashSet<EventPostDetail>();
        }

        public long Id { get; set; }
        public long? PostCategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PrizePool { get; set; }
        public long? Winner { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual PostCategory PostCategory { get; set; }
        public virtual ICollection<EventPostDetail> EventPostDetails { get; set; }
    }
}
