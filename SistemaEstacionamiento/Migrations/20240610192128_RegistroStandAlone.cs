using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstacionamiento.Migrations
{
    /// <inheritdoc />
    public partial class RegistroStandAlone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registro_Vehiculo",
                table: "Registro");

            migrationBuilder.DropIndex(
                name: "IX_Registro_Matricula",
                table: "Registro");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Registro_Matricula",
                table: "Registro",
                column: "Matricula");

            migrationBuilder.AddForeignKey(
                name: "FK_Registro_Vehiculo",
                table: "Registro",
                column: "Matricula",
                principalTable: "Vehiculo",
                principalColumn: "Matricula");
        }
    }
}
