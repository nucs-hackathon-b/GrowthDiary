using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthDiary.Models
{
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreationTime { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTime LastModifiedTime { get; set; }

        public int? InReplyToId { get; set; }

        public virtual Post InReplyTo { get; set; }
        public virtual ICollection<Post> Replies { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Content { get; set; }

        public int Like { get; set;  }

        public int Comments { get; set; }

<<<<<<< HEAD
        public virtual ICollection<Comment> CommentCollection { get; set; } = default;


=======
        public virtual ICollection<PostTag> PostTags { get; set; }
>>>>>>> 2098b81d95d603e7429bb015b110ba32117a9517


        public virtual ICollection<PostImage> Images { get; set; }
    }
}
