﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistemaOrdenes.Models;

#nullable disable

namespace SistemaOrdenes.Migrations
{
    [DbContext(typeof(DbProyectoAnalisisIiContext))]
    partial class DbProyectoAnalisisIiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SistemaOrdenes.Models.TbHistorial", b =>
                {
                    b.Property<int>("IdHistorial")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Historial");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdHistorial"));

                    b.Property<string>("Comentarios")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime")
                        .HasColumnName("Fecha_Actualizacion");

                    b.Property<int>("IdOrden")
                        .HasColumnType("int")
                        .HasColumnName("ID_Orden");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int")
                        .HasColumnName("ID_Usuario");

                    b.HasKey("IdHistorial")
                        .HasName("PK__tb_Histo__ECA89454C6124D95");

                    b.HasIndex("IdOrden");

                    b.HasIndex("IdUsuario");

                    b.ToTable("tb_Historial", (string)null);
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbOrden", b =>
                {
                    b.Property<int>("IdOrden")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Orden");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdOrden"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<string>("Detalles")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime")
                        .HasColumnName("Fecha_Creacion");

                    b.Property<int>("IdUsuarioComprador")
                        .HasColumnType("int")
                        .HasColumnName("ID_UsuarioComprador");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("NombreArticulo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Nombre_Articulo");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<decimal?>("Total")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("decimal(21, 2)")
                        .HasComputedColumnSql("([Precio]*[Cantidad])", false);

                    b.HasKey("IdOrden")
                        .HasName("PK__tb_Orden__EC9FA9497E7DE9B5");

                    b.HasIndex("IdUsuarioComprador");

                    b.ToTable("tb_Orden", (string)null);
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbRole", b =>
                {
                    b.Property<int>("IdRol")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Rol");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdRol"));

                    b.Property<decimal?>("MontoMaximo")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("Monto_Maximo");

                    b.Property<decimal?>("MontoMinimo")
                        .HasColumnType("decimal(10, 2)")
                        .HasColumnName("Monto_Minimo");

                    b.Property<string>("NombreRol")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.HasKey("IdRol")
                        .HasName("PK__tb_Roles__202AD22065D5D1D5");

                    b.ToTable("tb_Roles", (string)null);
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbUsuario", b =>
                {
                    b.Property<int>("IdUsuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID_Usuario");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdUsuario"));

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<bool?>("Confirmado")
                        .HasColumnType("bit");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("IdJefe")
                        .HasColumnType("int")
                        .HasColumnName("ID_Jefe");

                    b.Property<int>("IdRol")
                        .HasColumnType("int")
                        .HasColumnName("ID_Rol");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool?>("Restablecer")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Usuario")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("IdUsuario")
                        .HasName("PK__tb_Usuar__DE4431C5DCE1285C");

                    b.HasIndex("IdJefe");

                    b.HasIndex("IdRol");

                    b.HasIndex(new[] { "Correo" }, "UQ__tb_Usuar__60695A19E2F5984F")
                        .IsUnique();

                    b.HasIndex(new[] { "Usuario" }, "UQ__tb_Usuar__E3237CF76C8CE073")
                        .IsUnique();

                    b.ToTable("tb_Usuario", (string)null);
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbHistorial", b =>
                {
                    b.HasOne("SistemaOrdenes.Models.TbOrden", "IdOrdenNavigation")
                        .WithMany("TbHistorials")
                        .HasForeignKey("IdOrden")
                        .IsRequired()
                        .HasConstraintName("FK__tb_Histor__ID_Or__6C190EBB");

                    b.HasOne("SistemaOrdenes.Models.TbUsuario", "IdUsuarioNavigation")
                        .WithMany("TbHistorials")
                        .HasForeignKey("IdUsuario")
                        .IsRequired()
                        .HasConstraintName("FK__tb_Histor__ID_Us__6D0D32F4");

                    b.Navigation("IdOrdenNavigation");

                    b.Navigation("IdUsuarioNavigation");
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbOrden", b =>
                {
                    b.HasOne("SistemaOrdenes.Models.TbUsuario", "IdUsuarioCompradorNavigation")
                        .WithMany("TbOrdens")
                        .HasForeignKey("IdUsuarioComprador")
                        .IsRequired()
                        .HasConstraintName("FK__tb_Orden__ID_Usu__693CA210");

                    b.Navigation("IdUsuarioCompradorNavigation");
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbUsuario", b =>
                {
                    b.HasOne("SistemaOrdenes.Models.TbUsuario", "IdJefeNavigation")
                        .WithMany("InverseIdJefeNavigation")
                        .HasForeignKey("IdJefe")
                        .HasConstraintName("FK__tb_Usuari__ID_Je__66603565");

                    b.HasOne("SistemaOrdenes.Models.TbRole", "IdRolNavigation")
                        .WithMany("TbUsuarios")
                        .HasForeignKey("IdRol")
                        .IsRequired()
                        .HasConstraintName("FK__tb_Usuari__ID_Ro__656C112C");

                    b.Navigation("IdJefeNavigation");

                    b.Navigation("IdRolNavigation");
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbOrden", b =>
                {
                    b.Navigation("TbHistorials");
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbRole", b =>
                {
                    b.Navigation("TbUsuarios");
                });

            modelBuilder.Entity("SistemaOrdenes.Models.TbUsuario", b =>
                {
                    b.Navigation("InverseIdJefeNavigation");

                    b.Navigation("TbHistorials");

                    b.Navigation("TbOrdens");
                });
#pragma warning restore 612, 618
        }
    }
}
