using Microsoft.EntityFrameworkCore.Migrations;

namespace mdijk.f1.domain.Migrations
{
    public partial class eventssystem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RaceEvents",
                columns: table => new
                {
                    RaceEventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SequenceNumber = table.Column<int>(nullable: false),
                    RaceEventName = table.Column<string>(nullable: true),
                    CircuitId = table.Column<int>(nullable: false),
                    SeasonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaceEvents", x => x.RaceEventId);
                    table.ForeignKey(
                        name: "FK_RaceEvents_Circuits_CircuitId",
                        column: x => x.CircuitId,
                        principalTable: "Circuits",
                        principalColumn: "CircuitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RaceEvents_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "SeasonId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RaceEvents_CircuitId",
                table: "RaceEvents",
                column: "CircuitId");

            migrationBuilder.CreateIndex(
                name: "IX_RaceEvents_SeasonId",
                table: "RaceEvents",
                column: "SeasonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RaceEvents");
        }
    }
}
