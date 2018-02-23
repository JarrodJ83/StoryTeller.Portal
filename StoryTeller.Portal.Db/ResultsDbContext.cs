using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace StoryTeller.Portal.Db
{
    public class ResultsDbContext : DbContext
    {
        public DbSet<App> Apps { get; set; }
        public DbSet<Run> Runs { get; set; }
        public DbSet<Spec> Specs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=results.db");
        }
    }
}
