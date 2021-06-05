using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using geckserver.Data;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class StoryDto
    {
        public string postUid { get; set; }
        public string PostCategoryUid { get; set; }
        public string slug { get; set; }
        public string location { get; set; }
        public string caption { get; set; }
        public List<IFormFile> files { get; set; }
    }

    public partial class WeekylyStoryDto
    {
        public long weeklyId { get; set; }
        public string categoryUid { get; set; }
        public string location { get; set; }
        public string caption { get; set; }
        public List<IFormFile> files { get; set; }
    }

    public partial class PostDataChallengeDetailDto
    {
        public long WeeklyId { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PostDataChallengeDto> PostWeekly { get; set; }
    }

    public partial class PostDataChallengeDto
    {
        public string PostUid { get; set; }
        public string CategoryName { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Caption { get; set; }
        public long PostLikes { get; set; }
        public List<ImageList> Images { get; set; }
        public List<ReturnPostTagDto> Tags { get; set; }
    }

    public partial class PostDataCategoryDto
    {
        public string PostUid { get; set; }
        public string CategoryName { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Caption { get; set; }
        public long PostLikes { get; set; }
        public List<ImageList> Images { get; set; }
        public List<ReturnPostTagDto> Tags { get; set; }
    }

    public partial class PostGalleryDto
    {
        public string PostUid { get; set; }
        public string CategoryName { get; set; }
        public string Username { get; set; }
        public string Location { get; set; }
        public string Caption { get; set; }
        public List<ImageList> Images { get; set; }
    }

    public partial class PostLikeDto
    {
        public string postUid { get; set; }
    }

    public partial class PostReportDto
    {
        public string postUid { get; set; }
        public string usernameReport { get; set; }
        public string reason { get; set; }
    }

    public partial class PostReportDataDto
    {
        public string postUid { get; set; }
        public string username { get; set; }
        public string reportBy { get; set; }
        public string reason { get; set; }
        public List<ImageList> images { get; set; }
    }

    public partial class DeclinePostReportDto
    {
        public long postReportId { get; set; }
    }

    public partial class PostikesDataDto
    {
        public string username { get; set; }
    }

    public partial class ReturnPostTagDto
    {
        public long tagId { get; set; }
        public string name { get; set; }
    }

    public partial class ReturnPostLikeDto
    {
        public string username { get; set; }
    }

    public partial class ReturnPostImagesDto
    {
        public string postUid { get; set; }
        public List<ImageList> images { get; set; }
    }

    public partial class ImageList
    {
        public long Id { get; set; }
        public string imageType { get; set; }
        public string path { get; set; }
    }

    public partial class TotalLikes
    {
        public string Username { get; set; }
        public long totalLikes { get; set; }
    }

    public partial class PostDetailDto
    {
        public string postUid { get; set; }
        public string location { get; set; }
        public string caption { get; set; }
        public string userName { get; set; }
        public string categoryUid { get; set; }
        public string category { get; set; }
        public List<ReturnPostLikeDto> PostLikes { get; set; }
        public List<ReturnPostTagDto> TagData { get; set; }
        public List<ImageList> PostImages { get; set; }
        public List<PostDetailDto> OtherPost { get; set; }

    }
}