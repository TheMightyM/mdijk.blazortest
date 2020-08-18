using Microsoft.EntityFrameworkCore.Migrations;

namespace mdijk.f1.domain.Migrations
{
    public partial class pitstops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pitstops",
                columns: table => new
                {
                    PitstopId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RaceEventId = table.Column<int>(nullable: false),
                    DriverEntryId = table.Column<int>(nullable: false),
                    LapNumber = table.Column<int>(nullable: false),
                    Milliseconds = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitstops", x => x.PitstopId);
                    table.ForeignKey(
                        name: "FK_Pitstops_DriverEntries_DriverEntryId",
                        column: x => x.DriverEntryId,
                        principalTable: "DriverEntries",
                        principalColumn: "DriverEntryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pitstops_RaceEvents_RaceEventId",
                        column: x => x.RaceEventId,
                        principalTable: "RaceEvents",
                        principalColumn: "RaceEventId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pitstops_DriverEntryId",
                table: "Pitstops",
                column: "DriverEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitstops_RaceEventId",
                table: "Pitstops",
                column: "RaceEventId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pitstops");
        }
    }
}
