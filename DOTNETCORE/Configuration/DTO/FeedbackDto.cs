using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using geckserver.Data;
using Microsoft.AspNetCore.Http;

namespace geckserver.Configuration.DTO
{
    public partial class FeebackDto
    {
        public string subject { get; set; }
        public string message { get; set; }
    }
}