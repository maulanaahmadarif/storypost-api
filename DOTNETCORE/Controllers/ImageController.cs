using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace geckserver.Controllers
{
    [Route("storage/[controller]")]
    [ApiController]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ImageController : ControllerBase
    {
        public static IWebHostEnvironment _webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("{type}/{filename}")]
        public IActionResult Get(string type, string filename)
        {
            string path = _webHostEnvironment.WebRootPath;
            string fullPath = string.Empty;
            bool imageType = false;
            if (type.Equals("thumb"))
            {
                imageType = true;
                fullPath = Path.Combine(path, "images", "story", "small") + "//" + filename;
            }
            else if (type.Equals("feed"))
            {
                imageType = true;
                fullPath = Path.Combine(path, "images", "story", "medium") + "//" + filename;
            }
            else if (type.Equals("preview"))
            {
                imageType = true;
                fullPath = Path.Combine(path, "images", "story", "large") + "//" + filename;
            }
            else if (type.Equals("image"))
            {
                imageType = true;
                fullPath = Path.Combine(path, "images", "story", "original") + "//" + filename;
            }

            if (imageType && System.IO.File.Exists(fullPath))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(fullPath);
                return File(bytes, "image/jpeg");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}