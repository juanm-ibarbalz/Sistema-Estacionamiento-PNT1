using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaEstacionamiento.Models;

public partial class DbestacionamientoContext : DbContext
{
    public DbestacionamientoContext()
    {
    }

    public DbestacionamientoContext(DbContextOptions<DbestacionamientoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Registro> Registros { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Vehiculo> Vehiculos { get; set; }

    public virtual DbSet<Tarifa> Tarifas { get; set; }

    public virtual DbSet<Dimension> Dimensiones { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //Database.EnsureDeleted();
        //Database.EnsureCreated();
    }
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("server=DESKTOP-2SRAQ1R\\SQLEXPRESS; database=DBEstacionamiento; integrated security=true; TrustServerCertificate=Yes");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.Dni);

            entity.ToTable("Cliente");

            entity.Property(e => e.Dni)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("DNI");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Cuil);

            entity.ToTable("Empleado");

            entity.Property(e => e.Cuil)
                .HasColumnType("numeric(11, 0)")
                .HasColumnName("CUIL");
            entity.Property(e => e.Apellido)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsFixedLength();
        });

        modelBuilder.Entity<Registro>(entity =>
        {
            entity.HasKey(e => e.CodigoRegistro);

            entity.ToTable("Registro");

            entity.Property(e => e.CodigoRegistro).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Matricula)
                .HasMaxLength(7)
                .IsFixedLength();

            entity.Property(e => e.Importe).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.CodigoTicket);

            entity.ToTable("Ticket");

            entity.Property(e => e.CodigoTicket).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Dni)
                .HasColumnType("numeric(10, 0)")
                .HasColumnName("DNI");
            entity.Property(e => e.Matricula)
                .HasMaxLength(7)
                .IsFixedLength();

            entity.HasOne(d => d.DniNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Dni)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Cliente");

            entity.HasOne(d => d.MatriculaNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.Matricula)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Vehiculo");
        });

        modelBuilder.Entity<Vehiculo>(entity =>
        {
            entity.HasKey(e => e.Matricula);

            entity.ToTable("Vehiculo");

            entity.Property(e => e.Matricula)
                .HasMaxLength(7)
                .IsFixedLength();
            entity.Property(e => e.Color)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.Tipo)
                .HasMaxLength(4)
                .IsFixedLength();
        });

        modelBuilder.Entity<Tarifa>(entity =>
        {
            entity.HasKey(e => e.CodigoTarifa);

            entity.ToTable("Tarifa");

            entity.Property(e => e.CodigoTarifa)
                .HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Hora).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Dimension>(entity =>
        {
            entity.HasKey(e => e.CodigoDimension);

            entity.ToTable("Dimension");

            entity.Property(e => e.CodigoDimension)
                .HasColumnType("numeric(18, 0)");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
