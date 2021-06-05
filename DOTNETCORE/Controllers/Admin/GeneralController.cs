using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using geckserver.Configuration.DTO;
using geckserver.Data;
using geckserver.Utils;
using geckserver.Utils.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace geckserver.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private readonly StoryPostV2Context _context;

        public GeneralController(StoryPostV2Context context)
        {
            _context = context;
        }

        [HttpGet("postReport")]
        public async Task<ActionResult<PostReportDataDto>> getPostReport([FromQuery] PaginationFilter filter)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);
                if (userData.RoleId != 2)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Access denied for decline report post"
                    });
                }

                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var pagedData = await _context.PostReports
                .Include(e => e.PostData)
                    .ThenInclude(e => e.PostImages)
                .Include(e => e.User)
                .OrderBy(c => c.CreatedAt)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

                List<PostReportDataDto> listPostReport = new List<PostReportDataDto>();
                foreach (var item in pagedData)
                {
                    List<ImageList> imageList = new List<ImageList>();
                    foreach (var image in item.PostData.PostImages)
                    {
                        ImageList imageData = new ImageList();
                        imageData.Id = image.Id;
                        imageData.imageType = "thumbnail";
                        imageData.path = image.Path;
                        imageList.Add(imageData);
                    }

                    PostReportDataDto postReportDataDto = new PostReportDataDto();
                    postReportDataDto.postUid = item.PostData.Uid;
                    postReportDataDto.username = item.PostData.User.Username;
                    postReportDataDto.reportBy = item.User.Username;
                    postReportDataDto.reason = item.Reason;
                    postReportDataDto.images = imageList;
                    listPostReport.Add(postReportDataDto);
                }

                var totalRecords = pagedData.Count;
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<PostReportDataDto>>(listPostReport, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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

        [HttpPost("reportPostBanned")]
        public async Task<ActionResult<PostReport>> bannedReport([FromBody] DeclinePostReportDto data)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);
                if (userData.RoleId != 2)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Access denied for decline report post"
                    });
                }

                var postReport = _context.PostReports.FirstOrDefault(item => item.Id == data.postReportId);
                var postData = _context.PostData.FirstOrDefault(item => item.Id == postReport.PostDataId);

                using var transaction = _context.Database.BeginTransaction();
                try
                {

                    postReport.StatusDesc = "Post Deleted";
                    postReport.IsRemove = 1;
                    _context.PostReports.Update(postReport);
                    await _context.SaveChangesAsync();

                    postData.DeletedAt = DateTime.Now;
                    _context.PostData.Update(postData);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return Ok(new Response()
                    {
                        Status = true,
                        Message = "Post has been deleted"
                    });
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new Exception(e.Message);
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

        [HttpPost("reportPostDecline")]
        public async Task<ActionResult<PostReport>> declineReport([FromBody] DeclinePostReportDto data)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);
                if (userData.RoleId != 2)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Access denied for decline report post"
                    });
                }

                var postReport = _context.PostReports.FirstOrDefault(item => item.Id == data.postReportId);
                if (postReport == null)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "Post Report data not found"
                    });
                }

                postReport.IsDecline = 1;

                // update report post data
                _context.PostReports.Update(postReport);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Report declined submitted"
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

        [HttpGet("feedback")]
        public async Task<ActionResult<IEnumerable<Feedback>>> getFeedBack([FromQuery] PaginationFilter filter)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);
                if (userData.RoleId != 2)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Access denied for Feedback data"
                    });
                }

                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var pagedData = await _context.Feedbacks
                .OrderBy(c => c.CreatedAt)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

                var totalRecords = pagedData.Count;
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<Feedback>>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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

        [HttpGet("story/totalPhoto")]
        public async Task<ActionResult<PostImage>> getTotalPhoto([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate)
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

        [HttpGet("story/totalLike")]
        public async Task<ActionResult<PostImage>> getTotalLike([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate)
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

                var builder = _context.PostLikes;
                if (startDate != null && endDate != null)
                {
                }
                var data = await builder.CountAsync();

                return Ok(new ResponseCount()
                {
                    Status = true,
                    Message = "Total Like",
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

        [HttpGet("totalUser")]
        public async Task<ActionResult<PostImage>> getTotalUser([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate)
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

                var builder = _context.Users
                .Where(e => e.Status == "1")
                .Where(e => e.Confirmed == 1)
                .Where(e => e.IsBanned == 0)
                .Where(e => e.DeletedAt == null);
                if (startDate != null && endDate != null)
                {
                }
                var data = await builder.CountAsync();

                return Ok(new ResponseCount()
                {
                    Status = true,
                    Message = "Total User",
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
    }
}