using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class CategoryDto
    {
        public string Name { get; set; }
        public string Uid { get; set; }
        public int IsReported { get; set; }
    }
}