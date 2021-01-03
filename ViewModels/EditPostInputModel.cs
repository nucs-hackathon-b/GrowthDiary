using GrowthDiary.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthDiary.ViewModels
{
    public class EditPostInputModel
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "The {0} field is required.")]
        public string Content { get; set; }



        public  IList<string> ImageUrls { get; set; }

        public IList<IFormFile> Files { get; set; }

        public IList<string> ImagesToRemove { get; set; }
    }
}
