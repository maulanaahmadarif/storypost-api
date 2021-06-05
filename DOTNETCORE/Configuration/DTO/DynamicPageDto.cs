using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using geckserver.Data;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class ContentDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public partial class FaqDto
    {
        public long ContentId { get; set; }
        public int Ordering { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    public partial class MoveFaqDto
    {
        public long ContentId { get; set; }
        public long UpContentId { get; set; }
        public long DownContentId { get; set; }
        public string MoveType { get; set; }
    }
}