using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaEstacionamiento.Migrations
{
    /// <inheritdoc />
    public partial class AddImporteToRegistroAndDniNotNullableToTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    DNI = table.Column<decimal>(type: "numeric(10,0)", nullable: false),
                    Nombre = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    Apellido = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.DNI);
                });

            migrationBuilder.CreateTable(
                name: "Empleado",
                columns: table => new
                {
                    CUIL = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    Nombre = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    Apellido = table.Column<string>(type: "nchar(20)", fixedLength: true, maxLength: 20, nullable: false),
                    Sueldo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleado", x => x.CUIL);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculo",
                columns: table => new
                {
                    Matricula = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: false),
                    Tipo = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false),
                    Color = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: true),
                    Piso = table.Column<byte>(type: "tinyint", nullable: false),
                    Lugar = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculo", x => x.Matricula);
                });

            migrationBuilder.CreateTable(
                name: "Registro",
                columns: table => new
                {
                    CodigoRegistro = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    FechaEntrada = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraEntrada = table.Column<TimeOnly>(type: "time", nullable: false),
                    FechaSalida = table.Column<DateOnly>(type: "date", nullable: true),
                    HoraSalida = table.Column<TimeOnly>(type: "time", nullable: true),
                    Matricula = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: false),
                    Importe = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registro", x => x.CodigoRegistro);
                    table.ForeignKey(
                        name: "FK_Registro_Vehiculo",
                        column: x => x.Matricula,
                        principalTable: "Vehiculo",
                        principalColumn: "Matricula");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    CodigoTicket = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    FechaEntrada = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraEntrada = table.Column<TimeOnly>(type: "time", nullable: false),
                    Matricula = table.Column<string>(type: "nchar(7)", fixedLength: true, maxLength: 7, nullable: false),
                    DNI = table.Column<decimal>(type: "numeric(10,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.CodigoTicket);
                    table.ForeignKey(
                        name: "FK_Ticket_Cliente",
                        column: x => x.DNI,
                        principalTable: "Cliente",
                        principalColumn: "DNI");
                    table.ForeignKey(
                        name: "FK_Ticket_Vehiculo",
                        column: x => x.Matricula,
                        principalTable: "Vehiculo",
                        principalColumn: "Matricula");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registro_Matricula",
                table: "Registro",
                column: "Matricula");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_DNI",
                table: "Ticket",
                column: "DNI");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_Matricula",
                table: "Ticket",
                column: "Matricula");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Empleado");

            migrationBuilder.DropTable(
                name: "Registro");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "Vehiculo");
        }
    }
}
