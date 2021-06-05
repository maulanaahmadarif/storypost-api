using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class TagStatisticDto
    {
        public long tagId { get; set; }
        public string tagName { get; set; }

        public long totalPost { get; set; }
    }

    public partial class LikeStatisticDto
    {
        public long PostId { get; set; }
        public string Path { get; set; }
        public string PathType { get; set; }
        public string PostedBy { get; set; }
        public long TotalLike { get; set; }
    }
}