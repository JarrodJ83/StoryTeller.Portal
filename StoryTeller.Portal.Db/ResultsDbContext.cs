using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Storyteller.Portal.Db.Model;
using System;
using System.IO;

namespace StoryTeller.Portal.Db
{
    public class ResultsDbContext : DbContext
    {
        public DbSet<App> Apps { get; set; }
        public DbSet<Run> Runs { get; set; }
        public DbSet<Spec> Specs { get; set; }
        public DbSet<RunSpec> RunSpecs { get; set; }
        public DbSet<RunResult> RunResults { get; set; }

        public ResultsDbContext(DbContextOptions<ResultsDbContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var internalBuilder = modelBuilder.GetInfrastructure<InternalModelBuilder>();
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                internalBuilder
                    .Entity(entity.Name, ConfigurationSource.Convention)
                    .Relational(ConfigurationSource.Convention)
                    .ToTable(entity.ClrType.Name);
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer($"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Results;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
