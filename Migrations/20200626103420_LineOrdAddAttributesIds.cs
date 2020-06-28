using Microsoft.EntityFrameworkCore.Migrations;

namespace ClaveSol.Migrations
{
    public partial class LineOrdAddAttributesIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AtributsId",
                table: "LineOrder",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AtributsId",
                table: "LineOrder");
        }
    }
}
