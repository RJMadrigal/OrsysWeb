using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaOrdenes.Migrations
{
    /// <inheritdoc />
    public partial class estadoOrden : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "tb_Orden",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "tb_Orden");
        }
    }
}
