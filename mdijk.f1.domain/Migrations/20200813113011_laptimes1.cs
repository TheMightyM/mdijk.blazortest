using Microsoft.EntityFrameworkCore.Migrations;

namespace mdijk.f1.domain.Migrations
{
    public partial class laptimes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Laptimes",
                columns: table => new
                {
                    LaptimeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceEventId = table.Column<int>(nullable: false),
                    DriverEntryId = table.Column<int>(nullable: false),
                    LapNumber = table.Column<int>(nullable: false),
                    CurrentPosition = table.Column<int>(nullable: false),
                    Milliseconds = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Laptimes", x => x.LaptimeId);
                    table.ForeignKey(
                        name: "FK_Laptimes_DriverEntries_DriverEntryId",
                        column: x => x.DriverEntryId,
                        principalTable: "DriverEntries",
                        principalColumn: "DriverEntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Laptimes_RaceEvents_RaceEventId",
                        column: x => x.RaceEventId,
                        principalTable: "RaceEvents",
                        principalColumn: "RaceEventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Laptimes_DriverEntryId",
                table: "Laptimes",
                column: "DriverEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Laptimes_RaceEventId",
                table: "Laptimes",
                column: "RaceEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Laptimes");
        }
    }
}
