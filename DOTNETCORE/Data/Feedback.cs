using System;
using System.Collections.Generic;

#nullable disable

namespace geckserver.Data
{
    public partial class Feedback
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
