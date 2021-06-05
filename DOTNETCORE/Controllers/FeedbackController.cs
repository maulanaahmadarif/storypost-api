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
    public class FeedbackController : ControllerBase
    {
        private readonly StoryPostV2Context _context;
        public static IWebHostEnvironment _webHostEnvironment;

        public FeedbackController(StoryPostV2Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("submitFeedback")]
        public async Task<ActionResult<Feedback>> submitFeedback([FromBody] FeebackDto data)
        {
            try
            {

                var feedback = new Feedback();
                feedback.Subject = data.subject;
                feedback.Message = data.message;
                feedback.CreatedAt = DateTime.Now;

                _context.Feedbacks.Add(feedback);
                await _context.SaveChangesAsync();

                return Ok(new Response()
                {
                    Status = true,
                    Message = "Feedback submitted"
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