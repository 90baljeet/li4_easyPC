using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace easyPC.Migrations
{
    /// <inheritdoc />
    public partial class AddMontagemIdToEncomenda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MontagemId",
                table: "Encomendas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MontagemId",
                table: "Encomendas");
        }
    }
}
