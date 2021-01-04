using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthDiary.Models
{
    public class Tag
    {
        [Required]
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<PostTag> PostTags { get; set; }

    }
}
