using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GrowthDiary.Models;

namespace GrowthDiary.Data
{
    public class GrowthDiaryContext:DbContext
    {
        public GrowthDiaryContext(DbContextOptions<GrowthDiaryContext> options)
            : base(options)
        {
        }
        public DbSet<GrowthDiary.Models.Post> Post { get; set; }
    }
}
