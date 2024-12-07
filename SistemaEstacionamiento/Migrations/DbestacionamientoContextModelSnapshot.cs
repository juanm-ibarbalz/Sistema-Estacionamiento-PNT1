﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaEstacionamiento.Models;

#nullable disable

namespace SistemaEstacionamiento.Migrations
{
    [DbContext(typeof(DbestacionamientoContext))]
    partial class DbestacionamientoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaEstacionamiento.Models.Cliente", b =>
                {
                    b.Property<decimal>("Dni")
                        .HasColumnType("numeric(10, 0)")
                        .HasColumnName("DNI");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nchar(20)")
                        .IsFixedLength();

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nchar(20)")
                        .IsFixedLength();

                    b.HasKey("Dni");

                    b.ToTable("Cliente", (string)null);
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Dimension", b =>
                {
                    b.Property<decimal>("CodigoDimension")
                        .HasColumnType("numeric(18, 0)");

                    b.Property<int>("EspaciosAuto")
                        .HasColumnType("int");

                    b.Property<int>("EspaciosMoto")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<int>("Pisos")
                        .HasColumnType("int");

                    b.HasKey("CodigoDimension");

                    b.ToTable("Dimension", (string)null);
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Empleado", b =>
                {
                    b.Property<decimal>("Cuil")
                        .HasColumnType("numeric(11, 0)")
                        .HasColumnName("CUIL");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nchar(20)")
                        .IsFixedLength();

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nchar(20)")
                        .IsFixedLength();

                    b.Property<int>("Sueldo")
                        .HasColumnType("int");

                    b.HasKey("Cuil");

                    b.ToTable("Empleado", (string)null);
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Registro", b =>
                {
                    b.Property<decimal>("CodigoRegistro")
                        .HasColumnType("numeric(18, 0)");

                    b.Property<DateOnly>("FechaEntrada")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("FechaSalida")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("HoraEntrada")
                        .HasColumnType("time");

                    b.Property<TimeOnly?>("HoraSalida")
                        .HasColumnType("time");

                    b.Property<decimal?>("Importe")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Matricula")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nchar(7)")
                        .IsFixedLength();

                    b.HasKey("CodigoRegistro");

                    b.ToTable("Registro", (string)null);
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Tarifa", b =>
                {
                    b.Property<decimal>("CodigoTarifa")
                        .HasColumnType("numeric(18, 0)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Hora")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CodigoTarifa");

                    b.ToTable("Tarifa", (string)null);
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Ticket", b =>
                {
                    b.Property<decimal>("CodigoTicket")
                        .HasColumnType("numeric(18, 0)");

                    b.Property<decimal>("Dni")
                        .HasColumnType("numeric(10, 0)")
                        .HasColumnName("DNI");

                    b.Property<DateOnly>("FechaEntrada")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("HoraEntrada")
                        .HasColumnType("time");

                    b.Property<string>("Matricula")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nchar(7)")
                        .IsFixedLength();

                    b.HasKey("CodigoTicket");

                    b.HasIndex("Dni");

                    b.HasIndex("Matricula");

                    b.ToTable("Ticket", (string)null);
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Vehiculo", b =>
                {
                    b.Property<string>("Matricula")
                        .HasMaxLength(7)
                        .HasColumnType("nchar(7)")
                        .IsFixedLength();

                    b.Property<string>("Color")
                        .HasMaxLength(20)
                        .HasColumnType("nchar(20)")
                        .IsFixedLength();

                    b.Property<int>("Lugar")
                        .HasColumnType("int");

                    b.Property<int>("Piso")
                        .HasColumnType("int");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nchar(4)")
                        .IsFixedLength();

                    b.HasKey("Matricula");

                    b.ToTable("Vehiculo", (string)null);
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Ticket", b =>
                {
                    b.HasOne("SistemaEstacionamiento.Models.Cliente", "DniNavigation")
                        .WithMany("Tickets")
                        .HasForeignKey("Dni")
                        .IsRequired()
                        .HasConstraintName("FK_Ticket_Cliente");

                    b.HasOne("SistemaEstacionamiento.Models.Vehiculo", "MatriculaNavigation")
                        .WithMany("Tickets")
                        .HasForeignKey("Matricula")
                        .IsRequired()
                        .HasConstraintName("FK_Ticket_Vehiculo");

                    b.Navigation("DniNavigation");

                    b.Navigation("MatriculaNavigation");
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Cliente", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("SistemaEstacionamiento.Models.Vehiculo", b =>
                {
                    b.Navigation("Tickets");
                });
#pragma warning restore 612, 618
        }
    }
}
