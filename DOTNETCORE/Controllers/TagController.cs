using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using geckserver.Configuration.DTO;
using geckserver.Data;
using geckserver.Utils;
using geckserver.Utils.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace geckserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TagController : ControllerBase
    {
        private readonly StoryPostV2Context _context;
        public TagController(StoryPostV2Context context)
        {
            // _categoriesServices = categoriesServices;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetTags([FromQuery(Name = "search")] string search, [FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var builder = _context.TagData;
                if (search != null)
                {
                    builder.FromSqlRaw("SELECT * FROM dbo.TagData WHERE tagName LIKE '" + search + "%'");
                }
                builder.OrderBy(c => c.TagName)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize);

                var pagedData = await builder.ToListAsync();

                List<TagDto> tagData = new List<TagDto>();
                foreach (var item in pagedData)
                {
                    TagDto data = new TagDto();
                    data.tagId = item.Id;
                    data.tagName = item.TagName;
                    tagData.Add(data);
                }

                var totalRecords = pagedData.Count;
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<TagDto>>(tagData, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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

        [HttpGet("{id}")]
        public async Task<ActionResult<PostTag>> GetTags(long id)
        {
            var tag = await _context.PostTags.Where(e => e.Id == id).FirstOrDefaultAsync();

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }
    }
}