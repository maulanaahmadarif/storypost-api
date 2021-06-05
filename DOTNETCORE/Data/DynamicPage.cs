using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class DynamicPage
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Type { get; set; }
        public int Ordering { get; set; }
    }
}
