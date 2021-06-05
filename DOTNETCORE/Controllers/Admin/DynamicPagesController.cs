using System.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
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
    public class DynamicPagesController : ControllerBase
    {
        private readonly StoryPostV2Context _context;

        public DynamicPagesController(StoryPostV2Context context)
        {
            _context = context;
        }

        [HttpGet("getContent")]
        public async Task<ActionResult<IEnumerable<ContentDto>>> getContent()
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

                var getData = await _context.DynamicPages
                .Where(e => e.Type == "CONTENT")
                .ToListAsync();

                if (getData == null)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "Content not found"
                    });
                }

                List<ContentDto> listData = new List<ContentDto>();
                foreach (var item in getData)
                {
                    ContentDto data = new ContentDto();
                    data.Id = item.Id;
                    data.Title = item.Title;
                    data.Content = item.Content;
                    listData.Add(data);
                }
                return listData;
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

        [HttpGet("getFaq")]
        public async Task<ActionResult<IEnumerable<FaqDto>>> getFaq()
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

                var getData = await _context.DynamicPages
                .Where(e => e.Type == "FAQ")
                .OrderBy(e => e.Ordering)
                .ToListAsync();

                if (getData == null)
                {
                    return Ok(new Response()
                    {
                        Status = false,
                        Message = "FAQ not found"
                    });
                }

                List<FaqDto> listData = new List<FaqDto>();
                foreach (var item in getData)
                {
                    FaqDto getFaqData = JsonSerializer.Deserialize<FaqDto>(item.Content);

                    FaqDto data = new FaqDto();
                    data.ContentId = item.Id;
                    data.Ordering = item.Ordering;
                    data.Question = getFaqData.Question;
                    data.Answer = getFaqData.Answer;
                    listData.Add(data);
                }
                return listData;

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

        [HttpPost("addContent")]
        public async Task<ActionResult<IEnumerable<DynamicPage>>> addContent([FromBody] ContentDto body)
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

                var data = new DynamicPage();
                data.Type = "CONTENT";
                data.Title = body.Title;
                data.Content = body.Content;
                data.Ordering = 0;
                data.CreatedAt = DateTime.Now;

                _context.DynamicPages.Add(data);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Content added"
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

        [HttpPost("addFaq")]
        public async Task<ActionResult<IEnumerable<DynamicPage>>> addFaq([FromBody] FaqDto body)
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

                var lastData = await _context.DynamicPages
                .Where(e => e.Type == "FAQ")
                .OrderByDescending(e => e.Ordering)
                .FirstOrDefaultAsync();

                int ordering = 1;
                if (lastData != null)
                {
                    ordering = lastData.Ordering + 1;
                }

                var weatherForecast = new FaqDto
                {
                    Question = body.Question,
                    Answer = body.Answer
                };

                string contentData = JsonSerializer.Serialize(weatherForecast);

                var data = new DynamicPage();
                data.Type = "FAQ";
                data.Title = "";
                data.Content = contentData;
                data.Ordering = ordering;
                data.CreatedAt = DateTime.Now;

                _context.DynamicPages.Add(data);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "FAQ added"
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

        [HttpPost("orderingFaq")]
        public async Task<ActionResult<IEnumerable<DynamicPage>>> orderingFaq([FromBody] MoveFaqDto body)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var currentContent = await _context.DynamicPages.Where(e => e.Id == body.ContentId).FirstOrDefaultAsync();
                var changedContentBuilder = _context.DynamicPages;
                if (body.MoveType.Equals("UP"))
                {
                    changedContentBuilder.Where(e => e.Id == body.UpContentId);
                }

                if (body.MoveType.Equals("DOWN"))
                {
                    changedContentBuilder.Where(e => e.Id == body.DownContentId);
                }

                var changedContent = await changedContentBuilder.FirstOrDefaultAsync();

                if (currentContent == null && changedContent == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Content not found"
                    });
                }

                int currentOrder = changedContent.Ordering;
                int changeOrder = currentContent.Ordering;

                currentContent.Ordering = currentOrder;
                _context.DynamicPages.Update(currentContent);
                _context.SaveChanges();

                changedContent.Ordering = changeOrder;
                _context.DynamicPages.Update(changedContent);
                _context.SaveChanges();

                transaction.Commit();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "FAQ has been reordering"
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
        public async Task<ActionResult<IEnumerable<DynamicPage>>> deleteContent([FromBody] ContentDto body)
        {
            try
            {
                var contentData = await _context.DynamicPages.Where(e => e.Id == body.Id).FirstOrDefaultAsync();
                if (contentData == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Content not found"
                    });
                }

                contentData.DeletedAt = DateTime.Now;
                _context.DynamicPages.Update(contentData);
                _context.SaveChanges();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Content Deleted"
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