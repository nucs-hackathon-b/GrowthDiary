using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GrowthDiary.Models;

namespace GrowthDiary.Data
{
    public class GrowthDiaryContext : DbContext
    {
        public GrowthDiaryContext(DbContextOptions<GrowthDiaryContext> options)
            : base(options)
        {
        }
        public DbSet<GrowthDiary.Models.Post> Post { get; set; }
        public DbSet<GrowthDiary.Models.PostImage> PostImage { get; set; }
        public DbSet<GrowthDiary.Models.Comment> Comment { get; set; }
        public DbSet<GrowthDiary.Models.Tag> Tag { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<PostTag>(b =>
            {
                b.HasKey(pt => new { pt.PostId, pt.TagId });
                b.HasOne(pt => pt.Post).WithMany(p => p.PostTags).HasForeignKey(pt => pt.PostId);
                b.HasOne(pt => pt.Tag).WithMany(t => t.PostTags).HasForeignKey(pt => pt.TagId);
            });
        }
    }
}
