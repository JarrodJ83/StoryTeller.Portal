using Microsoft.EntityFrameworkCore;
using Storyteller.Portal.Db.Model;

namespace StoryTeller.Portal.Db
{
    public class ResultsDbContext : DbContext
    {
        public DbSet<App> Apps { get; set; }
        public DbSet<Run> Runs { get; set; }
        public DbSet<Spec> Specs { get; set; }
        public DbSet<RunSpec> RunSpecs { get; set; }
        public DbSet<RunResult> RunResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=results.db");
        }
    }
}
