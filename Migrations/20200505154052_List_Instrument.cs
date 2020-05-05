using Microsoft.EntityFrameworkCore.Migrations;

namespace ClaveSol.Migrations
{
    public partial class List_Instrument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "List",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InstrumentId",
                table: "Instrument",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "List_Instrument",
                columns: table => new
                {
                    ListId = table.Column<int>(nullable: false),
                    InstrumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_List_Instrument", x => new { x.ListId, x.InstrumentId });
                    table.ForeignKey(
                        name: "FK_List_Instrument_Instrument_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instrument",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_List_Instrument_List_ListId",
                        column: x => x.ListId,
                        principalTable: "List",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_List_Instrument_InstrumentId",
                table: "List_Instrument",
                column: "InstrumentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "List_Instrument");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "List");

            migrationBuilder.DropColumn(
                name: "InstrumentId",
                table: "Instrument");
        }
    }
}
