using Electricity.Domain.Entities;
using Electricity.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electricity.Infrastructure.Data
{
    public class ElectricityDbContext : DbContext
    {
        public ElectricityDbContext(DbContextOptions<ElectricityDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ElectricityData>()
                  .HasKey(x => new { x.Regions });
            modelBuilder.Entity<ElectricityData>()
                  .Property(e => e.PPlus)
                  .HasColumnType("decimal(18,5)");
            modelBuilder.Entity<ElectricityData>()
                  .Property(e => e.PMinus)
                  .HasColumnType("decimal(18,5)");
        }

        public DbSet<ElectricityData> ElectricityData { get; set; }
    }
}
