using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstacionamiento.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColorColumnLength : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Vehiculo",
                type: "nchar(20)",
                fixedLength: true,
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Vehiculo",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(20)",
                oldFixedLength: true,
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
