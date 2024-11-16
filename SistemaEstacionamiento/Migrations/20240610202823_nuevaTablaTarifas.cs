using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstacionamiento.Migrations
{
    /// <inheritdoc />
    public partial class nuevaTablaTarifas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Importe",
                table: "Registro",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Tarifa",
                columns: table => new
                {
                    CodigoTarifa = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    Hora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifa", x => x.CodigoTarifa);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarifa");

            migrationBuilder.AlterColumn<int>(
                name: "Importe",
                table: "Registro",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
