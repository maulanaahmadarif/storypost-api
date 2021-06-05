using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using geckserver.Configuration.DTO;
using geckserver.Data;
using geckserver.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using geckserver.Utils.Wrapper;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace geckserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WeeklyChallengeController : ControllerBase
    {
        private readonly StoryPostV2Context _context;
        public static IWebHostEnvironment _webHostEnvironment;

        public WeeklyChallengeController(StoryPostV2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost()]
        public async Task<ActionResult<IEnumerable<EventPost>>> addWeeklyChallenge([FromBody] WeeklyDto body)
        {
            try
            {
                var getRole = User.FindFirst("roleId")?.Value;
                if (Convert.ToInt32(getRole) != 2)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "You dont have permission"
                    });
                }

                var category = _context.PostCategories.FirstOrDefault(item => item.Uid == body.categoryUid);
                if (category == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Category not found"
                    });
                }

                var data = new EventPost();
                data.PostCategoryId = category.Id;
                data.Title = body.Title;
                data.Description = body.Description;
                data.StartDate = body.StartDate;
                data.EndDate = body.EndDate;
                data.PrizePool = body.PrizePool;
                data.Winner = 0;
                data.CreatedAt = DateTime.Now;

                _context.EventPosts.Add(data);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Weekly challenge added"
                });
            }
            catch (Exception e)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<WeeklyDataDto>>> getWeeklyChallenge([FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                DateTime dateNow = DateTime.Now;

                var weeklyBuilder = _context.EventPosts
                .Include(e => e.PostCategory)
                .Include(e => e.EventPostDetails)
                .ThenInclude(e => e.PostData)
                .Where(e => e.DeletedAt == null);

                var getActiveWeekly = await weeklyBuilder
                .Where(x => x.StartDate <= dateNow)
                .Where(x => x.EndDate >= dateNow)
                .FirstOrDefaultAsync();

                var recentEvent = await weeklyBuilder
                .OrderByDescending(e => e.CreatedAt)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
                // var QueryNew = _context.Appointments.Include(x => x.Employee).Include(x => x.city).Where(x => x.CreatedOn >= FromDate).Where(x => x.CreatedOn <= ToDate).Where(x => x.IsActive == true).ToList();


                long activeWeeklyId = 0;
                List<WeeklyDataDto> activeWeeklyData = new List<WeeklyDataDto>();
                WeeklyDataDto activeWeekly = new WeeklyDataDto();
                if (getActiveWeekly != null)
                {
                    DateTime startDate = Convert.ToDateTime(getActiveWeekly.StartDate);
                    DateTime endDate = Convert.ToDateTime(getActiveWeekly.EndDate);

                    var winner = string.Empty;
                    if (getActiveWeekly.Winner != 0)
                    {
                        var getWinner = await _context.Users.Where(e => e.Id == getActiveWeekly.Winner).FirstOrDefaultAsync();
                        winner = getWinner.Username;
                    }

                    activeWeeklyId = getActiveWeekly.Id;
                    activeWeekly.weeklyId = getActiveWeekly.Id;
                    activeWeekly.category = getActiveWeekly.PostCategory.Name;
                    activeWeekly.categoryUid = getActiveWeekly.PostCategory.Uid;
                    activeWeekly.Title = getActiveWeekly.Title;
                    activeWeekly.Description = getActiveWeekly.Description;
                    activeWeekly.StartDate = startDate.ToString("dd MMMM yyyy");
                    activeWeekly.EndDate = endDate.ToString("dd MMMM yyyy");
                    activeWeekly.PrizePool = getActiveWeekly.PrizePool;
                    activeWeekly.Winner = winner;
                    activeWeekly.TotalPost = getActiveWeekly.EventPostDetails.Count();
                    activeWeekly.RecentEvent = null;
                }
                else
                {
                    DateTime startDate = Convert.ToDateTime(recentEvent[0].StartDate);
                    DateTime endDate = Convert.ToDateTime(recentEvent[0].EndDate);

                    var winner = string.Empty;
                    if (recentEvent[0].Winner != 0)
                    {
                        var getWinner = await _context.Users.Where(e => e.Id == recentEvent[0].Winner).FirstOrDefaultAsync();
                        winner = getWinner.Username;
                    }

                    activeWeeklyId = recentEvent[0].Id;
                    activeWeekly.weeklyId = recentEvent[0].Id;
                    activeWeekly.category = recentEvent[0].PostCategory.Name;
                    activeWeekly.categoryUid = recentEvent[0].PostCategory.Uid;
                    activeWeekly.Title = recentEvent[0].Title;
                    activeWeekly.Description = recentEvent[0].Description;
                    activeWeekly.StartDate = startDate.ToString("dd MMMM yyyy");
                    activeWeekly.EndDate = endDate.ToString("dd MMMM yyyy");
                    activeWeekly.PrizePool = recentEvent[0].PrizePool;
                    activeWeekly.Winner = winner;
                    activeWeekly.TotalPost = recentEvent[0].EventPostDetails.Count();
                    activeWeekly.RecentEvent = null;
                }


                List<WeeklyPostDto> recentEventData = new List<WeeklyPostDto>();
                foreach (var item in recentEvent.Where(e => e.Id != activeWeeklyId))
                {
                    var winner = string.Empty;
                    if (item.Winner != 0)
                    {
                        var getWinner = await _context.Users.Where(e => e.Id == item.Winner).FirstOrDefaultAsync();
                        winner = getWinner.Username;
                    }

                    DateTime startDate = Convert.ToDateTime(item.StartDate);
                    DateTime endDate = Convert.ToDateTime(item.EndDate);

                    WeeklyPostDto dataEvent = new WeeklyPostDto();
                    dataEvent.weeklyId = item.Id;
                    dataEvent.category = item.PostCategory.Name;
                    dataEvent.categoryUid = item.PostCategory.Uid;
                    dataEvent.Title = item.Title;
                    dataEvent.Description = item.Description;
                    dataEvent.StartDate = startDate.ToString("dd MMMM yyyy");
                    dataEvent.EndDate = endDate.ToString("dd MMMM yyyy");
                    dataEvent.PrizePool = item.PrizePool;
                    dataEvent.Winner = winner;
                    dataEvent.TotalPost = item.EventPostDetails.Count();
                    recentEventData.Add(dataEvent);
                }

                activeWeekly.RecentEvent = recentEventData;
                activeWeeklyData.Add(activeWeekly);

                return activeWeeklyData;
            }
            catch (Exception e)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        [HttpGet("detail")]
        public async Task<ActionResult<IEnumerable<PostDataChallengeDetailDto>>> getWeeklyChallengeDetail([FromQuery(Name = "weeklyUid")] long weeklyUid, [FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var weeklyEvent = await _context.EventPosts.Include(e => e.PostCategory).FirstOrDefaultAsync(item => item.Id == weeklyUid);

                var pagedData = await _context.PostData
                .Include(e => e.PostCategory)
                .Include(e => e.User)
                .Include(e => e.PostImages)
                .Include(e => e.PostLikes)
                .Include(e => e.PostTags)
                .ThenInclude(e => e.TagData)
                .Where(e => e.DeletedAt == null)
                .Where(e => e.IsEvent == 1)
                .Where(e => e.WeeklyId == weeklyEvent.Id)
                .OrderByDescending(e => e.CreatedAt)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

                if (weeklyEvent == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Weekly Challenge not found"
                    });
                }

                List<PostDataChallengeDto> postList = new List<PostDataChallengeDto>();
                foreach (var item in pagedData)
                {
                    List<ImageList> imageList = new List<ImageList>();
                    foreach (var image in item.PostImages)
                    {
                        ImageList dataImage = new ImageList();
                        dataImage.Id = image.Id;
                        dataImage.path = image.Path;
                        dataImage.imageType = "thumbnail";
                        imageList.Add(dataImage);
                    }

                    List<ReturnPostTagDto> tagData = new List<ReturnPostTagDto>();
                    foreach (var itemTag in item.PostTags)
                    {
                        ReturnPostTagDto tag = new ReturnPostTagDto();
                        tag.tagId = itemTag.TagData.Id;
                        tag.name = itemTag.TagData.TagName;
                        tagData.Add(tag);
                    }

                    PostDataChallengeDto data = new PostDataChallengeDto();
                    data.PostUid = item.Uid;
                    data.CategoryName = item.PostCategory.Name;
                    data.Username = item.User.Username;
                    data.Location = item.Location;
                    data.Caption = item.Caption;
                    data.PostLikes = item.PostLikes.Count();
                    data.Images = imageList;
                    data.Tags = tagData;
                    postList.Add(data);
                }

                List<PostDataChallengeDetailDto> postWeeklyList = new List<PostDataChallengeDetailDto>();
                PostDataChallengeDetailDto postWeekly = new PostDataChallengeDetailDto();
                postWeekly.WeeklyId = weeklyEvent.Id;
                postWeekly.Category = weeklyEvent.PostCategory.Name;
                postWeekly.Title = weeklyEvent.Title;
                postWeekly.Description = weeklyEvent.Description;
                postWeekly.PostWeekly = postList;
                postWeeklyList.Add(postWeekly);

                var totalRecords = postList.Count();
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<PostDataChallengeDetailDto>>(postWeeklyList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
            }
            catch (Exception e)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        [HttpPost("submitWeekly")]
        public async Task<ActionResult<IEnumerable<PostData>>> postWeeklyChallenge([FromForm] WeekylyStoryDto data)
        {
            if (data.files.Count == 0)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = "No Image uploaded"
                });
            }

            var category = _context.PostCategories.FirstOrDefault(item => item.Uid == data.categoryUid);
            if (category == null)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = "No Category"
                });
            }

            var weeklyData = _context.EventPosts.FirstOrDefault(item => item.Id == data.weeklyId);
            if (weeklyData == null)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = "Weekly Challenge not found"
                });
            }

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                string path = _webHostEnvironment.WebRootPath;
                var smallPath = Path.Combine(path, "images", "story", "small");
                if (!Directory.Exists(smallPath))
                {
                    Directory.CreateDirectory(smallPath);
                }

                var mediumPath = Path.Combine(path, "images", "story", "medium");
                if (!Directory.Exists(mediumPath))
                {
                    Directory.CreateDirectory(mediumPath);
                }

                var largePath = Path.Combine(path, "images", "story", "large");
                if (!Directory.Exists(largePath))
                {
                    Directory.CreateDirectory(largePath);
                }
                var originalPath = Path.Combine(path, "images", "story", "original");
                if (!Directory.Exists(originalPath))
                {
                    Directory.CreateDirectory(originalPath);
                }

                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);

                var postData = new PostData();
                postData.UserId = userData.Id;
                postData.PostCategoryId = category.Id;
                postData.Uid = Convert.ToString(Guid.NewGuid());
                postData.Location = data.location;
                postData.Caption = data.caption;
                postData.Title = "";
                postData.IsEvent = 1;
                postData.WeeklyId = weeklyData.Id;
                postData.CreatedAt = DateTime.Now;

                _context.PostData.Add(postData);
                await _context.SaveChangesAsync();

                long insertedPostData = postData.Id;

                var eventDetail = new EventPostDetail();
                eventDetail.EventPostId = weeklyData.Id;
                eventDetail.PostDataId = insertedPostData;
                _context.EventPostDetails.Add(eventDetail);
                await _context.SaveChangesAsync();

                List<string> tagData = this.getTag(data.caption);

                for (int i = 0; i < tagData.Count; i++)
                {
                    var checkTagData = _context.TagData.FirstOrDefault(item => item.TagName == tagData[i].ToString());
                    long tagId = 0;
                    if (checkTagData == null)
                    {
                        var dataTag = new TagData();
                        dataTag.TagName = tagData[i];

                        _context.TagData.Add(dataTag);
                        await _context.SaveChangesAsync();
                        tagId = dataTag.Id;
                    }
                    else
                    {
                        tagId = checkTagData.Id;
                    }

                    var postTag = new PostTag();
                    postTag.TagDataId = tagId;
                    postTag.PostDataId = insertedPostData;

                    _context.PostTags.Add(postTag);
                    await _context.SaveChangesAsync();
                }

                foreach (var file in data.files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    // var fileExtension = Path.GetExtension(fileName);
                    var newFileName = String.Concat(myUniqueFileName, ".jpg");

                    // story original path
                    var orignalDestination = originalPath + "//" + newFileName;
                    using var image = Image.Load(file.OpenReadStream());
                    image.Mutate(x => x.Resize(image.Width, image.Height));
                    image.SaveAsJpeg(orignalDestination);

                    // story small path
                    var smallDestination = smallPath + "//" + newFileName;
                    image.Mutate(x => x.Resize(image.Width, image.Height));
                    image.SaveAsJpeg(smallDestination);

                    // story medium path
                    var mediumDestination = mediumPath + "//" + newFileName;
                    image.Mutate(x => x.Resize(image.Width, image.Height));
                    image.SaveAsJpeg(mediumDestination);

                    // story large path
                    var largeDestination = largePath + "//" + newFileName;
                    image.Mutate(x => x.Resize(image.Width, image.Height));
                    image.SaveAsJpeg(largeDestination);

                    var originalImage = new PostImage();
                    originalImage.PostDataId = insertedPostData;
                    originalImage.Path = newFileName;
                    originalImage.CreatedAt = DateTime.Now;

                    _context.PostImages.Add(originalImage);
                    await _context.SaveChangesAsync();
                }

                transaction.Commit();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Weekly added"
                });
            }
            catch (Exception e)
            {
                transaction.Rollback();
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        public List<string> getTag(string caption)
        {
            List<string> tagList = new List<string>();
            string[] splitCaption = caption.Split(' ');
            string check = string.Empty;
            for (int i = 0; i < splitCaption.Length; i++)
            {
                string findTag = splitCaption[i].Substring(0, 1);
                if (findTag.Equals("#"))
                {
                    check += splitCaption[i] + "-";
                    tagList.Add(splitCaption[i].Substring(1));
                }
            }
            return tagList;
        }
    }
}