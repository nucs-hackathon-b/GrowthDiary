using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthDiary.Models
{
    public class Comment
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ForWhichId { get; set; }

        public DateTime CreationTime { get; set; }

        [Required]
        public string Contents { get; set; }
    }
}
