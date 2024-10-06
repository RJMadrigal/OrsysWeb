using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaOrdenes.Migrations
{
    /// <inheritdoc />
    public partial class migracionPrueba : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
            name: "tb_Roles",
            columns: table => new
            {
                ID_Rol = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                NombreRol = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                Monto_Maximo = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                Monto_Minimo = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK__tb_Roles__202AD22065D5D1D5", x => x.ID_Rol);
            });


                

            migrationBuilder.CreateTable(
                name: "tb_Usuario",
                columns: table => new
                {
                    ID_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Usuario = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Clave = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Restablecer = table.Column<bool>(type: "bit", nullable: true),
                    Confirmado = table.Column<bool>(type: "bit", nullable: true),
                    Token = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    ID_Rol = table.Column<int>(type: "int", nullable: false),
                    ID_Jefe = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tb_Usuar__DE4431C5DCE1285C", x => x.ID_Usuario);
                    table.ForeignKey(
                        name: "FK__tb_Usuari__ID_Je__66603565",
                        column: x => x.ID_Jefe,
                        principalTable: "tb_Usuario",
                        principalColumn: "ID_Usuario");
                    table.ForeignKey(
                        name: "FK__tb_Usuari__ID_Ro__656C112C",
                        column: x => x.ID_Rol,
                        principalTable: "tb_Roles",
                        principalColumn: "ID_Rol");
                });

            migrationBuilder.CreateTable(
                name: "tb_Orden",
                columns: table => new
                {
                    ID_Orden = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre_Articulo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Modelo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Detalles = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Fecha_Creacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    ID_UsuarioComprador = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(21,2)", nullable: true, computedColumnSql: "([Precio]*[Cantidad])", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tb_Orden__EC9FA9497E7DE9B5", x => x.ID_Orden);
                    table.ForeignKey(
                        name: "FK__tb_Orden__ID_Usu__693CA210",
                        column: x => x.ID_UsuarioComprador,
                        principalTable: "tb_Usuario",
                        principalColumn: "ID_Usuario");
                });

            migrationBuilder.CreateTable(
                name: "tb_Historial",
                columns: table => new
                {
                    ID_Historial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Orden = table.Column<int>(type: "int", nullable: false),
                    ID_Usuario = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Fecha_Actualizacion = table.Column<DateTime>(type: "datetime", nullable: false),
                    Comentarios = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tb_Histo__ECA89454C6124D95", x => x.ID_Historial);
                    table.ForeignKey(
                        name: "FK__tb_Histor__ID_Or__6C190EBB",
                        column: x => x.ID_Orden,
                        principalTable: "tb_Orden",
                        principalColumn: "ID_Orden");
                    table.ForeignKey(
                        name: "FK__tb_Histor__ID_Us__6D0D32F4",
                        column: x => x.ID_Usuario,
                        principalTable: "tb_Usuario",
                        principalColumn: "ID_Usuario");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_Historial_ID_Orden",
                table: "tb_Historial",
                column: "ID_Orden");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Historial_ID_Usuario",
                table: "tb_Historial",
                column: "ID_Usuario");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Orden_ID_UsuarioComprador",
                table: "tb_Orden",
                column: "ID_UsuarioComprador");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Usuario_ID_Jefe",
                table: "tb_Usuario",
                column: "ID_Jefe");

            migrationBuilder.CreateIndex(
                name: "IX_tb_Usuario_ID_Rol",
                table: "tb_Usuario",
                column: "ID_Rol");

            migrationBuilder.CreateIndex(
                name: "UQ__tb_Usuar__60695A19E2F5984F",
                table: "tb_Usuario",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__tb_Usuar__E3237CF76C8CE073",
                table: "tb_Usuario",
                column: "Usuario",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_Historial");

            migrationBuilder.DropTable(
                name: "tb_Orden");

            migrationBuilder.DropTable(
                name: "tb_Usuario");

            migrationBuilder.DropTable(
                name: "tb_Roles");
        }
    }
}
