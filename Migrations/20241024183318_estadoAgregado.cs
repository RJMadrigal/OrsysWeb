using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaOrdenes.Migrations
{
    /// <inheritdoc />
    public partial class estadoAgregado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "tb_Orden",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
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
