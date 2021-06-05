using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using geckserver.Configuration.DTO;
using geckserver.Data;
using geckserver.Utils;
using geckserver.Utils.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace geckserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StoryController : ControllerBase
    {
        private readonly StoryPostV2Context _context;
        public static IWebHostEnvironment _webHostEnvironment;

        public StoryController(StoryPostV2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<PostData>>> PostData([FromForm] StoryDto data)
        {
            if (data.files.Count == 0)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = "No Image uploaded"
                });
            }

            var category = _context.PostCategories.FirstOrDefault(item => item.Uid == data.PostCategoryUid);
            if (category == null)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = "No Category"
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
                postData.IsEvent = 0;
                postData.WeeklyId = 0;
                postData.CreatedAt = DateTime.Now;

                _context.PostData.Add(postData);
                await _context.SaveChangesAsync();

                long insertedPostData = postData.Id;

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
                    Message = "Story added"
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

        [HttpPost("update")]
        public async Task<ActionResult<IEnumerable<PostData>>> PostDataUpdate([FromBody] StoryDto data)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var postData = _context.PostData.FirstOrDefault(item => item.Uid == data.postUid);
                if (postData == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Post not found"
                    });
                }

                // remove old tag
                var postId = new PostTag()
                {
                    PostDataId = postData.Id
                };
                _context.PostTags.Remove(postId);
                await _context.SaveChangesAsync();

                postData.Location = data.location;
                postData.Caption = data.caption;

                // update post data
                _context.PostData.Update(postData);
                await _context.SaveChangesAsync();

                List<string> tagData = this.getTag(data.caption);

                // insert new tag
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
                    postTag.PostDataId = postData.Id;

                    _context.PostTags.Add(postTag);
                    await _context.SaveChangesAsync();
                }

                transaction.Commit();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Post deleted"
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

        [HttpPost("delete")]
        public async Task<ActionResult<IEnumerable<PostData>>> PostDataDelete([FromBody] PostLikeDto data)
        {
            try
            {
                var postData = _context.PostData.FirstOrDefault(item => item.Uid == data.postUid);
                if (postData == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Post not found"
                    });
                }
                postData.DeletedAt = DateTime.Now;

                _context.PostData.Update(postData);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Post deleted"
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

        [HttpGet("category")]
        public async Task<ActionResult<IEnumerable<PostDataCategoryDto>>> getPostByCategory([FromQuery(Name = "categoryUid")] string categoryUid, [FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var category = _context.PostCategories.FirstOrDefault(item => item.Uid == categoryUid);
                if (category == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Category not found"
                    });
                }

                var pagedData = await _context.PostData
                .Include(e => e.PostCategory)
                .Include(e => e.User)
                .Include(e => e.PostImages)
                .Include(e => e.PostLikes)
                .Include(e => e.PostTags)
                .ThenInclude(e => e.TagData)
                .Where(e => e.DeletedAt == null)
                .Where(e => e.IsEvent == 0)
                .Where(e => e.PostCategoryId == category.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

                List<PostDataCategoryDto> postList = new List<PostDataCategoryDto>();
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

                    PostDataCategoryDto postDataCategoruDto = new PostDataCategoryDto();
                    postDataCategoruDto.PostUid = item.Uid;
                    postDataCategoruDto.CategoryName = item.PostCategory.Name;
                    postDataCategoruDto.Username = item.User.Username;
                    postDataCategoruDto.Location = item.Location;
                    postDataCategoruDto.Caption = item.Caption;
                    postDataCategoruDto.PostLikes = item.PostLikes.Count();
                    postDataCategoruDto.Images = imageList;
                    postDataCategoruDto.Tags = tagData;
                    postList.Add(postDataCategoruDto);
                }

                var totalRecords = await _context.PostData.CountAsync();
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<PostDataCategoryDto>>(postList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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
        public async Task<ActionResult<PostData>> getPostById([FromQuery(Name = "postUid")] string postUid, [FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var postData = await _context.PostData
                .Include(e => e.PostLikes)
                .Include(e => e.PostCategory)
                .Include(e => e.PostImages)
                .Include(e => e.User)
                .Include(e => e.PostTags)
                .ThenInclude(e => e.TagData)
                .Where(item => item.Uid == postUid).FirstOrDefaultAsync();

                if (postData == null)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "Post data not found"
                    });
                }

                var otherPostData = await _context.PostData
                .Include(e => e.PostLikes)
                .Include(e => e.PostCategory)
                .Include(e => e.PostImages)
                .Include(e => e.User)
                .Include(e => e.PostTags)
                .ThenInclude(e => e.TagData)
                .Where(e => e.PostCategoryId == postData.PostCategoryId)
                .Where(e => e.Id != postData.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .OrderByDescending(e => e.CreatedAt)
                .ToListAsync();

                List<ReturnPostTagDto> postTag = new List<ReturnPostTagDto>();
                foreach (var item in postData.PostTags)
                {
                    ReturnPostTagDto tagItem = new ReturnPostTagDto();
                    tagItem.tagId = item.TagData.Id;
                    tagItem.name = item.TagData.TagName;
                    postTag.Add(tagItem);
                }

                List<ReturnPostLikeDto> postLikes = new List<ReturnPostLikeDto>();
                foreach (var item in postData.PostLikes.Where(e => e.DeletedAt == null))
                {
                    ReturnPostLikeDto likeItem = new ReturnPostLikeDto();
                    likeItem.username = item.User.Username;
                    postLikes.Add(likeItem);
                }

                List<ImageList> postImages = new List<ImageList>();
                foreach (var item in postData.PostImages.Where(e => e.DeletedAt == null))
                {
                    ImageList imageItem = new ImageList();
                    imageItem.Id = item.Id;
                    imageItem.path = item.Path;
                    postImages.Add(imageItem);
                }

                // other Post by current category
                List<PostDetailDto> otherPostDataList = new List<PostDetailDto>();
                foreach (var item in otherPostData)
                {
                    List<ReturnPostTagDto> otherPostTag = new List<ReturnPostTagDto>();
                    foreach (var itemOther in item.PostTags)
                    {
                        ReturnPostTagDto otherTagItem = new ReturnPostTagDto();
                        otherTagItem.tagId = itemOther.TagData.Id;
                        otherTagItem.name = itemOther.TagData.TagName;
                        otherPostTag.Add(otherTagItem);
                    }

                    List<ReturnPostLikeDto> otherPostLikes = new List<ReturnPostLikeDto>();
                    foreach (var itemOther in item.PostLikes.Where(e => e.DeletedAt == null))
                    {
                        ReturnPostLikeDto likeItem = new ReturnPostLikeDto();
                        likeItem.username = itemOther.User.Username;
                        otherPostLikes.Add(likeItem);
                    }

                    List<ImageList> otherPostImages = new List<ImageList>();
                    foreach (var itemOther in item.PostImages.Where(e => e.DeletedAt == null))
                    {
                        ImageList imageItem = new ImageList();
                        imageItem.Id = itemOther.Id;
                        imageItem.path = itemOther.Path;
                        otherPostImages.Add(imageItem);
                    }

                    PostDetailDto data = new PostDetailDto();
                    data.postUid = item.Uid;
                    data.location = item.Location;
                    data.caption = item.Caption;
                    data.userName = item.User.Username;
                    data.categoryUid = item.PostCategory.Uid;
                    data.category = item.PostCategory.Name;
                    data.PostLikes = otherPostLikes;
                    data.TagData = otherPostTag;
                    data.PostImages = otherPostImages;
                    otherPostDataList.Add(data);
                }

                return Ok(
                    new PostDetailDto()
                    {
                        postUid = postData.Uid,
                        location = postData.Location,
                        caption = postData.Caption,
                        userName = postData.User.Username,
                        categoryUid = postData.PostCategory.Uid,
                        category = postData.PostCategory.Name,
                        PostLikes = postLikes,
                        TagData = postTag,
                        PostImages = postImages,
                        OtherPost = otherPostDataList,
                    }
                );
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

        [HttpPost("like")]
        public async Task<ActionResult<IEnumerable<PostLike>>> like([FromBody] PostLikeDto data)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);

                var postData = _context.PostData.FirstOrDefault(item => item.Uid == data.postUid);
                if (postData == null)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "Post data not found"
                    });
                }

                var postLikeData = _context.PostLikes.FirstOrDefault(e => e.UserId == userData.Id && e.PostDataId == postData.Id && e.DeletedAt == null);
                if (postLikeData == null)
                {
                    // like
                    var postLike = new PostLike();
                    postLike.UserId = userData.Id;
                    postLike.PostDataId = postData.Id;
                    postLike.CreatedAt = DateTime.Now;

                    _context.PostLikes.Add(postLike);
                    await _context.SaveChangesAsync();

                    return Ok(new Response()
                    {
                        Status = true,
                        Message = "Like added"
                    });
                }
                else
                {
                    // unlike
                    postLikeData.DeletedAt = DateTime.Now;

                    _context.PostLikes.Update(postLikeData);
                    await _context.SaveChangesAsync();

                    return Ok(new Response()
                    {
                        Status = true,
                        Message = "unlike success"
                    });
                }
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

        [HttpPost("unlike")]
        public async Task<ActionResult<IEnumerable<PostLike>>> unlike([FromBody] PostLikeDto data)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);
                var postData = _context.PostData.FirstOrDefault(item => item.Uid == data.postUid);
                var postLike = _context.PostLikes.FirstOrDefault(item => item.UserId == userData.Id && item.PostDataId == postData.Id);

                if (postLike == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Post Data not found"
                    });
                }
                postLike.DeletedAt = DateTime.Now;

                _context.PostLikes.Update(postLike);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "unlike success"
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

        [HttpGet("tag")]
        public async Task<ActionResult<IEnumerable<ReturnPostImagesDto>>> PostDataByTag([FromQuery(Name = "tagName")] string tagName, [FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var tagData = _context.TagData.FirstOrDefault(item => item.TagName == tagName);

                var pagedData = await _context.PostTags
                .Include(e => e.PostData)
                    .ThenInclude(e => e.PostImages)
                .Where(e => e.TagDataId == tagData.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

                List<ReturnPostImagesDto> imageByTag = new List<ReturnPostImagesDto>();
                foreach (var item in pagedData)
                {
                    List<ImageList> imageList = new List<ImageList>();
                    foreach (var imageData in item.PostData.PostImages)
                    {
                        ImageList image = new ImageList();
                        image.Id = imageData.Id;
                        image.imageType = "thumbnail";
                        image.path = imageData.Path;
                        imageList.Add(image);
                    }
                    ReturnPostImagesDto tagImage = new ReturnPostImagesDto();
                    tagImage.postUid = item.PostData.Uid;
                    tagImage.images = imageList;
                    imageByTag.Add(tagImage);
                }

                var totalRecords = imageByTag.Count;
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<ReturnPostImagesDto>>(imageByTag, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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

        [HttpPost("reportPost")]
        public async Task<ActionResult<PostReport>> reportPost([FromBody] PostReportDto data)
        {
            try
            {
                var postData = _context.PostData.FirstOrDefault(item => item.Uid == data.postUid);
                var userReport = _context.Users.FirstOrDefault(item => item.Username == data.usernameReport);
                if (postData == null)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "Post data not found"
                    });
                }

                if (userReport == null)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "User data not found"
                    });
                }

                var PostReport = new PostReport();
                PostReport.PostDataId = postData.Id;
                PostReport.UserId = userReport.Id;
                PostReport.Reason = data.reason;
                PostReport.StatusDesc = null;
                PostReport.IsDecline = null;
                PostReport.IsRemove = null;
                PostReport.IsEmailed = null;
                PostReport.CreatedAt = DateTime.Now;

                _context.PostReports.Add(PostReport);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Report submitted"
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

        [HttpGet("totalPhoto")]
        public async Task<ActionResult<IEnumerable<UserActivityDto>>> getTotalPhoto([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate)
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

                var builder = _context.PostImages;
                if (startDate != null && endDate != null)
                {
                }
                var data = await builder.CountAsync();

                return Ok(new ResponseCount()
                {
                    Status = true,
                    Message = "Total Photo",
                    Total = data.ToString(),
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