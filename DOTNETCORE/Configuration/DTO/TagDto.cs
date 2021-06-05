using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using geckserver.Data;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class TagDto
    {
        public long tagId { get; set; }
        public string tagName { get; set; }
    }

    public partial class StatisticHastagDto {
        public DateTime? PostDate { get; set; }
        public string TagName { get; set; }
        public long TotalPost { get; set; }
    }
}