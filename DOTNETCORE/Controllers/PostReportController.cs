using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using geckserver.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace geckserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostReportController : ControllerBase
    {
        private readonly StoryPostV2Context _context;
        public PostReportController(StoryPostV2Context context)
        {
            // _categoriesServices = categoriesServices;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostReport>>> GetPostReport(int? skip, int? take)
        {
            var postReport = _context.PostReports.AsQueryable();

            if (skip != null)
            {
                postReport = postReport.Skip((int)skip);
            }

            if (take != null)
            {
                postReport = postReport.Take((int)take);
            }

            return await postReport.ToListAsync();
        }
    }
}