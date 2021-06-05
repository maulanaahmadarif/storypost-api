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
    public class UsersController : ControllerBase
    {
        private readonly StoryPostV2Context _context;

        public UsersController(StoryPostV2Context context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUser([FromQuery] PaginationFilter pagination, [FromQuery(Name = "status")] int status, [FromQuery(Name = "roleId")] int roleId)
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

                var builder = _context.Users
                .Include(e => e.Role)
                .Where(e => e.DeletedAt == null);

                if (status != 0)
                    builder.Where(e => e.Status == status.ToString());

                if (roleId != 0)
                    builder.Where(e => e.RoleId == roleId);


                builder.Skip((validFilter.PageNumber - 1) * validFilter.PageSize);
                builder.Take(validFilter.PageSize);

                var pagedData = await builder.ToListAsync();

                var totalRecords = await _context.Users.CountAsync();
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<User>>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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

        [HttpPost("banned")]
        public ActionResult<IEnumerable<User>> BanUser([FromBody] BanUserDto data)
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

                var userData = _context.Users.FirstOrDefault(item => item.Id == data.id);
                if (userData == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "User not found"
                    });
                }

                userData.IsBanned = 1;

                _context.Users.Update(userData);
                _context.SaveChanges();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "User has been banned"
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

        [HttpPost("changePassword")]
        public ActionResult<IEnumerable<User>> ChangePassword([FromBody] ChangePasswordDto data)
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

                var userData = _context.Users.FirstOrDefault(item => item.Id == data.id);
                if (userData == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "User not found"
                    });
                }

                var hashing = new HashingManager();
                var hash = hashing.HashToString(data.password);
                userData.Password = hash;
                _context.Users.Update(userData);
                _context.SaveChanges();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Change password success"
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

        [HttpPost("update")]
        public ActionResult<IEnumerable<User>> UpdateUser([FromBody] UpdateUserDto data)
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

                var userData = _context.Users.FirstOrDefault(item => item.Id == data.Id);
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
                userData.Nip = data.Nip;
                userData.RoleId = data.RoleId;
                userData.UpdatedAt = DateTime.Now;

                _context.Users.Update(userData);
                _context.SaveChanges();

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

        [HttpGet("activity")]
        public async Task<ActionResult<IEnumerable<UserActivityDto>>> getUserActivity([FromQuery] PaginationFilter pagination)
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

                var pagedData = await _context.UserAcitivities
                .Include(e => e.User)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

                List<UserActivityDto> activityList = new List<UserActivityDto>();
                foreach (var item in pagedData)
                {
                    DateTime loginDate = Convert.ToDateTime(item.LastLogin);

                    UserActivityDto data = new UserActivityDto();
                    data.Username = item.User.Username;
                    data.LoginDate = loginDate.ToString("dddd, dd MMMM yyyy HH:mm:ss");
                    activityList.Add(data);
                }

                var totalRecords = await _context.Users.CountAsync();
                var totalPages = totalRecords % validFilter.PageSize;
                return Ok(new PagedResponse<List<UserActivityDto>>(activityList, validFilter.PageNumber, validFilter.PageSize, totalRecords, totalPages));
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