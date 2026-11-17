using GameOfLife.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Infrastructure.Persistence
{

    public class AppDbContext : DbContext
    {
        public DbSet<SimulationSessionEntity> Sessions => Set<SimulationSessionEntity>();
        public DbSet<MetricRecordEntity> MetricsHistory => Set<MetricRecordEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }



    }
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gameoflife_db;Username=postgres;Password=s100282s");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
