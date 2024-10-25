using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaOrdenes.Migrations
{
    /// <inheritdoc />
    public partial class nuevaTabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NivelesAprobacion",
                columns: table => new
                {
                    IdNivel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MontoMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MontoMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IdAprobadorFinanciero = table.Column<int>(type: "int", nullable: false),
                    AprobadorFinancieroIdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelesAprobacion", x => x.IdNivel);
                    table.ForeignKey(
                        name: "FK_NivelesAprobacion_tb_Usuario_AprobadorFinancieroIdUsuario",
                        column: x => x.AprobadorFinancieroIdUsuario,
                        principalTable: "tb_Usuario",
                        principalColumn: "ID_Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NivelesAprobacion_AprobadorFinancieroIdUsuario",
                table: "NivelesAprobacion",
                column: "AprobadorFinancieroIdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NivelesAprobacion");
        }
    }
}
