using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaOrdenes.Migrations
{
    /// <inheritdoc />
    public partial class agregoEstado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "tb_Usuario",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "tb_Usuario");
        }
    }
}
