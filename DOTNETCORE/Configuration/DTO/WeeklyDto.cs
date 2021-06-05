using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class WeeklyDto
    {
        public string categoryUid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PrizePool { get; set; }
        public string Winner { get; set; }
    }

    public partial class WeeklyDataDto
    {
        public long weeklyId { get; set; }
        public string category { get; set; }
        public string categoryUid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PrizePool { get; set; }
        public string Winner { get; set; }
        public long TotalPost { get; set; }
        public List<WeeklyPostDto> RecentEvent { get; set; }
    }

    public partial class WeeklyPostDto {
        public long weeklyId { get; set; }
        public string category { get; set; }
        public string categoryUid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PrizePool { get; set; }
        public string Winner { get; set; }
        public long TotalPost { get; set; }
    }
}