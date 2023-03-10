// <auto-generated />
using System;
using Electricity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Electricity.Infrastructure.Migrations
{
    [DbContext(typeof(ElectricityDbContext))]
    partial class ElectricityDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.1.23111.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Electricity.Domain.Entities.ElectricityData", b =>
                {
                    b.Property<string>("Regions")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OBJ_NUMERIS")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PMinus")
                        .HasColumnType("decimal(18,5)");

                    b.Property<decimal>("PPlus")
                        .HasColumnType("decimal(18,5)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Regions");

                    b.ToTable("ElectricityData");
                });
#pragma warning restore 612, 618
        }
    }
}
