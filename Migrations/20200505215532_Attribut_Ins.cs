using Microsoft.EntityFrameworkCore.Migrations;

namespace ClaveSol.Migrations
{
    public partial class Attribut_Ins : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "attrInsId",
                table: "Instrument",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attribut",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    AttributId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribut", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attribut_Ins",
                columns: table => new
                {
                    AttributId = table.Column<int>(nullable: false),
                    InstrumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attribut_Ins", x => new { x.AttributId, x.InstrumentId });
                    table.ForeignKey(
                        name: "FK_Attribut_Ins_Attribut_AttributId",
                        column: x => x.AttributId,
                        principalTable: "Attribut",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attribut_Ins_Instrument_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attribut_Ins_InstrumentId",
                table: "Attribut_Ins",
                column: "InstrumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attribut_Ins");

            migrationBuilder.DropTable(
                name: "Attribut");

            migrationBuilder.DropColumn(
                name: "attrInsId",
                table: "Instrument");
        }
    }
}
