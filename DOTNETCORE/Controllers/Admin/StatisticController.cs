using System.ComponentModel.DataAnnotations;
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
    public class StatisticController : ControllerBase
    {
        private readonly StoryPostV2Context _context;

        public StatisticController(StoryPostV2Context context)
        {
            _context = context;
        }

        [HttpGet("hastags")]
        public async Task<ActionResult<IEnumerable<TagData>>> getHastags([FromQuery(Name = "startDate")] string startDate, [FromQuery(Name = "endDate")] string endDate, [FromQuery(Name = "moreData")] string moreData)
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

                var pagedData = await _context.TagData
                .Include(e => e.PostTags)
                    .ThenInclude(e => e.PostData)
                .ToListAsync();

                List<StatisticHastagDto> listData = new List<StatisticHastagDto>();
                foreach (var item in pagedData)
                {
                    StatisticHastagDto data = new StatisticHastagDto();
                    data.TagName = "#" + item.TagName;
                    data.TotalPost = item.PostTags.Count();
                    listData.Add(data);
                }

                // if (startDate != null && endDate != null)
                // {
                //     items.Where(i => i.CreatedDate.Date >= DateX && i.CreatedDate.Date <= DateY);
                // }

                listData.Sort(delegate (StatisticHastagDto x, StatisticHastagDto y)
                {
                    return y.TotalPost.CompareTo(x.TotalPost);
                });

                List<StatisticHastagDto> skippedData = null;
                if (moreData.Equals("0") && listData.Count > 10)
                {
                    skippedData = listData.GetRange(0, 10);
                }

                return Ok(new PagedResponse<List<StatisticHastagDto>>(listData, 0, 0, listData.Count, 0));
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

        [HttpGet("likes")]
        public async Task<ActionResult<IEnumerable<PostData>>> getLikes([FromQuery(Name = "moreData")] string moreData, [FromQuery] PaginationFilter pagination)
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
                var validFilter = new PaginationFilter(pagination.PageNumber, pagination.PageSize);

                var pagedData = await _context.PostData
                .Include(e => e.PostLikes)
                .Include(e => e.PostImages)
                .Include(e => e.User)
                .ToListAsync();

                List<LikeStatisticDto> listData = new List<LikeStatisticDto>();
                foreach (var item in pagedData)
                {
                    var data = new LikeStatisticDto();
                    data.PostId = item.Id;
                    data.PostedBy = item.User.Name;
                    data.PathType = "thumbnail";
                    data.Path = item.PostImages.FirstOrDefault().Path;
                    data.TotalLike = item.PostLikes.Count();
                    listData.Add(data);
                }

                listData.Sort(delegate (LikeStatisticDto x, LikeStatisticDto y)
                {
                    return y.TotalLike.CompareTo(x.TotalLike);
                });

                List<LikeStatisticDto> skippedData = null;
                if (moreData.Equals("0") && listData.Count > 10)
                {
                    skippedData = listData.GetRange(0, 10);
                }

                var totalRecords = await _context.Users.CountAsync();
                var totalPages = totalRecords % validFilter.PageSize;

                return Ok(new PagedResponse<List<LikeStatisticDto>>(listData, 0, 0, listData.Count, 0));
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