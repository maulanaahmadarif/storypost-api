

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
using Microsoft.Extensions.FileProviders;

namespace geckserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProfileController : ControllerBase
    {
        private readonly StoryPostV2Context _context;
        public static IWebHostEnvironment _webHostEnvironment;

        public ProfileController(StoryPostV2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // get profile
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<User>>> GetProfile([FromQuery(Name = "username")] string username)
        {
            try
            {
                var userData = await _context.Users.Where(e => e.Username == username).FirstOrDefaultAsync();

                if (userData == null)
                {
                    return NotFound();
                }

                return Ok(
                    new UserProfileDTO()
                    {
                        Username = userData.Username,
                        Name = userData.Name,
                        Phone = userData.Phone,
                        Facebook = userData.Facebook,
                        Twitter = userData.Twitter,
                        Instagram = userData.Instagram,
                        Picture = userData.Picture,
                        Email = userData.Email,
                        Nip = userData.Nip,
                        RoleId = "0"
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

        [HttpPost("update")]
        public async Task<ActionResult<IEnumerable<User>>> UpdateProfile([FromBody] UserProfileUpdateDto data)
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                // var userData = await _context.Users.Where(e => e.Email == userEmail).FirstOrDefaultAsync();
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);

                if (userData == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "User not found"
                    });
                }

                userData.Name = data.Name;
                userData.Phone = data.Phone;
                userData.Facebook = data.Facebook;
                userData.Twitter = data.Twitter;
                userData.Instagram = data.Instagram;

                _context.Users.Update(userData);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Profile updated"
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


        [HttpPost("changepicture")]
        public async Task<ActionResult<IEnumerable<User>>> UpdatePicture([FromForm] ProfilePictureDto data)
        {
            try
            {
                if (data.file.Length > 0)
                {
                    string path = _webHostEnvironment.WebRootPath;
                    var filepath = Path.Combine(path, "Images", "Profile");
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }

                    var fileName = Path.GetFileName(data.file.FileName);
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                    var fileExtension = Path.GetExtension(fileName);
                    var newFileName = String.Concat(myUniqueFileName, fileExtension);

                    var savedPath = filepath + "//" + newFileName;

                    using (FileStream fileStream = System.IO.File.Create(savedPath))
                    {
                        await data.file.CopyToAsync(fileStream);
                        await fileStream.FlushAsync();

                        return Ok(new Response()
                        {
                            Status = true,
                            Message = "Picture updated"
                        });
                    }
                }
                else
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "No file uploaded"
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

        [HttpGet("totalLike")]
        public async Task<ActionResult<IEnumerable<ReturnPostImagesDto>>> totalLike()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userData = _context.Users.FirstOrDefault(item => item.Email == userEmail);

                var pagedData = await _context.PostData
                .Include(e => e.PostLikes)
                .Where(e => e.UserId == userData.Id)
                .ToListAsync();

                long totalLike = 0;
                foreach (var item in pagedData)
                {
                    totalLike += item.PostLikes.Count();
                }

                return Ok(new TotalLikes()
                {
                    Username = userData.Username,
                    totalLikes = totalLike
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