using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthDiary.ViewModels
{
    public class PostInputModel
    {
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Content { get; set; }


        public int? InReplyToId { get; set; }

        public IList<IFormFile> Files { get; set; }
    }
}
