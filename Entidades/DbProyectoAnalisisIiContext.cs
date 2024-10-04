using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaOrdenes.Entidades;

public partial class DbProyectoAnalisisIiContext : DbContext
{
    public DbProyectoAnalisisIiContext()
    {
    }

    public DbProyectoAnalisisIiContext(DbContextOptions<DbProyectoAnalisisIiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbHistorial> TbHistorials { get; set; }

    public virtual DbSet<TbOrden> TbOrdens { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    public virtual DbSet<TbUsuario> TbUsuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbHistorial>(entity =>
        {
            entity.HasKey(e => e.IdHistorial).HasName("PK__tb_Histo__ECA89454C6124D95");

            entity.ToTable("tb_Historial");

            entity.Property(e => e.IdHistorial).HasColumnName("ID_Historial");
            entity.Property(e => e.Comentarios)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FechaActualizacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Actualizacion");
            entity.Property(e => e.IdOrden).HasColumnName("ID_Orden");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.TbHistorials)
                .HasForeignKey(d => d.IdOrden)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Histor__ID_Or__6C190EBB");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbHistorials)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Histor__ID_Us__6D0D32F4");
        });

        modelBuilder.Entity<TbOrden>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PK__tb_Orden__EC9FA9497E7DE9B5");

            entity.ToTable("tb_Orden");

            entity.Property(e => e.IdOrden).HasColumnName("ID_Orden");
            entity.Property(e => e.Detalles)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.IdUsuarioComprador).HasColumnName("ID_UsuarioComprador");
            entity.Property(e => e.Modelo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreArticulo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Nombre_Articulo");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Total)
                .HasComputedColumnSql("([Precio]*[Cantidad])", false)
                .HasColumnType("decimal(21, 2)");

            entity.HasOne(d => d.IdUsuarioCompradorNavigation).WithMany(p => p.TbOrdens)
                .HasForeignKey(d => d.IdUsuarioComprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Orden__ID_Usu__693CA210");
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__tb_Roles__202AD22065D5D1D5");

            entity.ToTable("tb_Roles");

            entity.Property(e => e.IdRol).HasColumnName("ID_Rol");
            entity.Property(e => e.MontoMaximo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Monto_Maximo");
            entity.Property(e => e.MontoMinimo)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Monto_Minimo");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__tb_Usuar__DE4431C5DCE1285C");

            entity.ToTable("tb_Usuario");

            entity.HasIndex(e => e.Correo, "UQ__tb_Usuar__60695A19E2F5984F").IsUnique();

            entity.HasIndex(e => e.Usuario, "UQ__tb_Usuar__E3237CF76C8CE073").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdJefe).HasColumnName("ID_Jefe");
            entity.Property(e => e.IdRol).HasColumnName("ID_Rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Token)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Usuario)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdJefeNavigation).WithMany(p => p.InverseIdJefeNavigation)
                .HasForeignKey(d => d.IdJefe)
                .HasConstraintName("FK__tb_Usuari__ID_Je__66603565");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TbUsuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Usuari__ID_Ro__656C112C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
