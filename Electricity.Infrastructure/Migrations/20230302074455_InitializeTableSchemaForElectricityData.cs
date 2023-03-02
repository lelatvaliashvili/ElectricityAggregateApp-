using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electricity.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitializeTableSchemaForElectricityData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ElectricityData",
                columns: table => new
                {
                    Regions = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OBJ_NUMERIS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PPlus = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PMinus = table.Column<decimal>(type: "decimal(18,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectricityData", x => x.Regions);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ElectricityData");
        }
    }
}
