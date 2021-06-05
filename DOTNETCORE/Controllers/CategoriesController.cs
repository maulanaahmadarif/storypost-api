using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using geckserver.Configuration.DTO;
using geckserver.Data;
using geckserver.Services;
using geckserver.Utils;
using geckserver.Utils.Wrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace geckserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        private readonly StoryPostV2Context _context;
        public CategoriesController(StoryPostV2Context context)
        {
            // _categoriesServices = categoriesServices;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostCategory>>> GetCategories([FromQuery] PaginationFilter filter)
        {
            try
            {
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var pagedData = await _context.PostCategories.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();

                List<CategoryDto> categoryData = new List<CategoryDto>();
                foreach (var item in pagedData)
                {
                    CategoryDto data = new CategoryDto();
                    data.Uid = item.Uid;
                    data.Name = item.Name;
                    categoryData.Add(data);
                }

                var totalRecords = categoryData.Count;
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<CategoryDto>>(categoryData, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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

        private string generateSlug(string str)
        {
            string[] getName = str.ToLower().Split(' ');
            string generateSlug = string.Empty;
            for (int i = 0; i < getName.Length; i++)
            {
                generateSlug += "-" + getName[i];
            }
            return generateSlug.Remove(0, 1);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> AddCategory([FromBody] CategoryDto data)
        {
            try
            {
                var slug = this.generateSlug(data.Name);
                object checkSlug = await _context.PostCategories.Where(e => e.Slug == slug).FirstOrDefaultAsync();
                if (checkSlug != null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Category is exist"
                    });
                }

                var category = new PostCategory();
                category.Uid = Convert.ToString(Guid.NewGuid());
                category.Slug = slug;
                category.Name = data.Name;
                category.IsReported = data.IsReported;
                category.CreatedAt = DateTime.Now;

                _context.PostCategories.Add(category);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "new category recorded"
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

        [HttpPut("update")]
        public async Task<ActionResult<IEnumerable<User>>> UpdateCategory([FromQuery(Name = "uid")] string uid, [FromBody] CategoryDto data)
        {
            var category = _context.PostCategories.FirstOrDefault(item => item.Uid == uid);

            if (category == null)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = "Category not found"
                });
            }
            var newSlug = this.generateSlug(data.Name);
            object checkSlug = await _context.PostCategories.Where(e => e.Slug == newSlug).FirstOrDefaultAsync();
            if (checkSlug != null)
            {
                return BadRequest(new Response()
                {
                    Status = false,
                    Message = "Category is exist"
                });
            }

            category.Slug = newSlug;
            category.Name = data.Name;

            _context.Entry(category).Property(x => x.Slug).IsModified = true;
            _context.Entry(category).Property(x => x.Name).IsModified = true;
            _context.PostCategories.Update(category);
            _context.SaveChanges();

            return Ok(new Response()
            {
                Status = true,
                Message = "Category updated"
            });

        }

        [HttpGet("{uid}")]
        public async Task<ActionResult<PostCategory>> GetCategories(string uid)
        {
            var category = await _context.PostCategories.Where(e => e.Uid == uid).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
    }
}