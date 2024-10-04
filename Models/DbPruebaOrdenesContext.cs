using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SistemaOrdenes.Models;

public partial class DbPruebaOrdenesContext : DbContext
{
    public DbPruebaOrdenesContext()
    {
    }

    public DbPruebaOrdenesContext(DbContextOptions<DbPruebaOrdenesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbArticulo> TbArticulos { get; set; }

    public virtual DbSet<TbFlujoEstado> TbFlujoEstados { get; set; }

    public virtual DbSet<TbFlujoFinanciero> TbFlujoFinancieros { get; set; }

    public virtual DbSet<TbOrden> TbOrdens { get; set; }

    public virtual DbSet<TbRole> TbRoles { get; set; }

    public virtual DbSet<Usuarios> TbUsuarios { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbArticulo>(entity =>
        {
            entity.HasKey(e => e.IdArticuloOrden).HasName("PK__tb_Artic__48A6D606148A2DB4");

            entity.ToTable("tb_Articulo");

            entity.Property(e => e.IdArticuloOrden).HasColumnName("ID_ArticuloOrden");
            entity.Property(e => e.Articulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IdOrden).HasColumnName("ID_Orden");
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.TbArticulos)
                .HasForeignKey(d => d.IdOrden)
                .HasConstraintName("FK__tb_Articu__ID_Or__6477ECF3");
        });

        modelBuilder.Entity<TbFlujoEstado>(entity =>
        {
            entity.HasKey(e => e.IdFlujoEstado).HasName("PK__tb_Flujo__5D6130BE6B663382");

            entity.ToTable("tb_FlujoEstado");

            entity.Property(e => e.IdFlujoEstado).HasColumnName("ID_FlujoEstado");
            entity.Property(e => e.Estado)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbFlujoFinanciero>(entity =>
        {
            entity.HasKey(e => e.IdFlujoFinanciero).HasName("PK__tb_Flujo__D163BCE02C6FD0CA");

            entity.ToTable("tb_FlujoFinanciero");

            entity.Property(e => e.IdFlujoFinanciero).HasColumnName("ID_FlujoFinanciero");
            entity.Property(e => e.IdRolAprobador).HasColumnName("ID_RolAprobador");
            entity.Property(e => e.MontoMaximo).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.MontoMinimo).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<TbOrden>(entity =>
        {
            entity.HasKey(e => e.IdOrden).HasName("PK__tb_Orden__EC9FA949549EAF33");

            entity.ToTable("tb_Orden", tb => tb.HasTrigger("tr_ActualizarFlujoFinanciero"));

            entity.Property(e => e.IdOrden).HasColumnName("ID_Orden");
            entity.Property(e => e.Comentario)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.FechaOrden).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IdEstado).HasColumnName("ID_Estado");
            entity.Property(e => e.IdFlujo).HasColumnName("ID_Flujo");
            entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.TbOrdens)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__tb_Orden__ID_Est__5FB337D6");

            entity.HasOne(d => d.IdFlujoNavigation).WithMany(p => p.TbOrdens)
                .HasForeignKey(d => d.IdFlujo)
                .HasConstraintName("FK__tb_Orden__ID_Flu__619B8048");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TbOrdens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__tb_Orden__ID_Usu__60A75C0F");
        });

        modelBuilder.Entity<TbRole>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__tb_Roles__202AD220BB9E9970");

            entity.ToTable("tb_Roles");

            entity.Property(e => e.IdRol).HasColumnName("ID_Rol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__tb_Usuar__DE4431C543DB2025");

            entity.ToTable("tb_Usuario");

            entity.Property(e => e.IdUsuario)
                .ValueGeneratedNever()
                .HasColumnName("ID_Usuario");
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
                .HasConstraintName("FK__tb_Usuari__ID_Je__4CA06362");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TbUsuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tb_Usuari__ID_Ro__4BAC3F29");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
