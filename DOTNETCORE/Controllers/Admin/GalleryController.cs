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
    public class GalleryController : ControllerBase
    {
        private readonly StoryPostV2Context _context;

        public GalleryController(StoryPostV2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostGalleryDto>>> getGallery([FromQuery] PaginationFilter pagination)
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
                .Include(e => e.User)
                .Include(e => e.PostImages)
                .Include(e => e.PostCategory)
                .Include(e => e.PostLikes)
                .Include(e => e.PostTags)
                .ThenInclude(e => e.TagData)
                .Where(e => e.DeletedAt == null)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

                List<PostGalleryDto> galleryList = new List<PostGalleryDto>();
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

                    PostGalleryDto postGallery = new PostGalleryDto();
                    postGallery.PostUid = item.Uid;
                    postGallery.CategoryName = item.PostCategory.Name;
                    postGallery.Username = item.User.Username;
                    postGallery.Location = item.Location;
                    postGallery.Caption = item.Caption;
                    postGallery.Images = imageList;
                    galleryList.Add(postGallery);
                }

                var totalRecords = await _context.Users.CountAsync();
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<PostGalleryDto>>(galleryList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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