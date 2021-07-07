using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using geckserver.Configuration;
using geckserver.Configuration.DTO;
using geckserver.Data;
using geckserver.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace geckserver.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static object _response;
        private readonly StoryPostV2Context _context;
        private readonly JwtConfig _jwtConfig;
        public AuthController(
            StoryPostV2Context context,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(User user)
        {
            try
            {
                object checkUsername = await _context.Users.Where(e => e.Username == user.Username).FirstOrDefaultAsync();
                object checkEmail = await _context.Users.Where(e => e.Email == user.Email).FirstOrDefaultAsync();
                if (checkUsername != null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Username already used"
                    });
                }

                if (checkEmail != null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Email already used"
                    });
                }
                
                string decryptPass = new HashingData().DecryptStringAES(user.Password);
                var hashing = new HashingManager();
                var hash = hashing.HashToString(decryptPass);
                user.Password = hash;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "User Registed"
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

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginDTO user)
        {
            try
            {
                var userData = await _context.Users.Where(e => e.Username == user.username).FirstOrDefaultAsync();
                if (userData == null)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Username not found"
                    });
                }

                if (userData.IsBanned == 1)
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "User is banned"
                    });
                }

                var hashing = new HashingManager();
                var result = hashing.Verify(user.password, userData.Password);
                var token = GenerateJwtToken(userData);

                var userProfile = new UserProfileDTO()
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
                    RoleId = userData.RoleId.ToString()
                };

                if (result)
                {
                    var userAcitivity = new UserAcitivity();
                    userAcitivity.UserId = userData.Id;
                    userAcitivity.LastLogin = DateTime.Now;

                    _context.UserAcitivities.Add(userAcitivity);
                    await _context.SaveChangesAsync();

                    return Ok(new AuthResult()
                    {
                        Success = true,
                        Token = token,
                        Profile = userProfile,
                    });
                }
                else
                {
                    return BadRequest(new Response()
                    {
                        Status = false,
                        Message = "Invalid Password"
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

        [HttpPost("getToken")]
        public async Task<ActionResult<AuthResult>> getToken([FromBody] GetTokenDto body)
        {
            try
            {
                // body.account value is email
                string decryptAccount = new HashingData().DecryptStringAES(body.account);

                // checked email
                var userData = await _context.Users.Where(e => e.Email == decryptAccount).FirstOrDefaultAsync();
                if (userData != null)
                {
                    // register
                    using var transaction = _context.Database.BeginTransaction();
                    try
                    {
                        var hashing = new HashingManager();
                        var hashPassword = hashing.HashToString(decryptAccount);

                        var user = new User();
                        user.Username = decryptAccount;
                        user.Password = hashPassword;
                        user.Name = decryptAccount;
                        user.Email = decryptAccount;
                        user.Status = "1";
                        user.RoleId = 1;
                        user.CreatedAt = DateTime.Now;

                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();

                        transaction.Commit();

                        var token = GenerateJwtToken(user);

                        var userProfile = new UserProfileDTO()
                        {
                            Username = user.Username,
                            Name = user.Name,
                            Phone = user.Phone,
                            Facebook = user.Facebook,
                            Twitter = user.Twitter,
                            Instagram = user.Instagram,
                            Picture = user.Picture,
                            Email = user.Email,
                            Nip = user.Nip,
                            RoleId = user.RoleId.ToString()
                        };

                        return Ok(new AuthResult()
                        {
                            Success = true,
                            Token = token,
                            Profile = userProfile,
                        });
                    }
                    catch (Exception err)
                    {
                        transaction.Rollback();
                        return BadRequest(new Response()
                        {
                            Status = false,
                            Message = err.Message
                        });
                    }
                } 
                else 
                {
                    var token = GenerateJwtToken(userData);

                    var userProfile = new UserProfileDTO()
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
                        RoleId = userData.RoleId.ToString()
                    };

                    return Ok(new AuthResult()
                    {
                        Success = true,
                        Token = token,
                        Profile = userProfile,
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

        private async Task<User> GetUser(string email, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
        private string GenerateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("roleId", user.RoleId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6), // 5-10 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }
    }
}