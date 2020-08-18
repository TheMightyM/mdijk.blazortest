using Microsoft.EntityFrameworkCore.Migrations;

namespace mdijk.f1.domain.Migrations
{
    public partial class results : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventResults",
                columns: table => new
                {
                    EventResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceEventId = table.Column<int>(nullable: false),
                    DriverEntryId = table.Column<int>(nullable: false),
                    FinishingPosition = table.Column<int>(nullable: false),
                    FastestLap = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventResults", x => x.EventResultId);
                    table.ForeignKey(
                        name: "FK_EventResults_DriverEntries_DriverEntryId",
                        column: x => x.DriverEntryId,
                        principalTable: "DriverEntries",
                        principalColumn: "DriverEntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventResults_RaceEvents_RaceEventId",
                        column: x => x.RaceEventId,
                        principalTable: "RaceEvents",
                        principalColumn: "RaceEventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventResultMetaDatas",
                columns: table => new
                {
                    EventResultMetaDataId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventResultId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventResultMetaDatas", x => x.EventResultMetaDataId);
                    table.ForeignKey(
                        name: "FK_EventResultMetaDatas_EventResults_EventResultId",
                        column: x => x.EventResultId,
                        principalTable: "EventResults",
                        principalColumn: "EventResultId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventResultMetaDatas_EventResultId",
                table: "EventResultMetaDatas",
                column: "EventResultId");

            migrationBuilder.CreateIndex(
                name: "IX_EventResults_DriverEntryId",
                table: "EventResults",
                column: "DriverEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_EventResults_RaceEventId",
                table: "EventResults",
                column: "RaceEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventResultMetaDatas");

            migrationBuilder.DropTable(
                name: "EventResults");
        }
    }
}
