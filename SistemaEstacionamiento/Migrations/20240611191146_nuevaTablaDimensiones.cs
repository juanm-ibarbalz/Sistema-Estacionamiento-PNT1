using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstacionamiento.Migrations
{
    /// <inheritdoc />
    public partial class nuevaTablaDimensiones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dimension",
                columns: table => new
                {
                    CodigoDimension = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    EspaciosAuto = table.Column<int>(type: "int", nullable: false),
                    EspaciosMoto = table.Column<int>(type: "int", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dimension", x => x.CodigoDimension);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dimension");
        }
    }
}
