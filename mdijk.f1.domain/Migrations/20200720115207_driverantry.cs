using Microsoft.EntityFrameworkCore.Migrations;

namespace mdijk.f1.domain.Migrations
{
    public partial class driverantry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Drivers_FirstDriverId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Drivers_SecondDriverId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_FirstDriverId",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_SecondDriverId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "FirstDriverId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "SecondDriverId",
                table: "Entries");

            migrationBuilder.CreateTable(
                name: "DriverEntries",
                columns: table => new
                {
                    DriverEntryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(nullable: false),
                    EntryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverEntries", x => x.DriverEntryId);
                    table.ForeignKey(
                        name: "FK_DriverEntries_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "DriverId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverEntries_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverEntries_DriverId",
                table: "DriverEntries",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverEntries_EntryId",
                table: "DriverEntries",
                column: "EntryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverEntries");

            migrationBuilder.AddColumn<int>(
                name: "FirstDriverId",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondDriverId",
                table: "Entries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_FirstDriverId",
                table: "Entries",
                column: "FirstDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SecondDriverId",
                table: "Entries",
                column: "SecondDriverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Drivers_FirstDriverId",
                table: "Entries",
                column: "FirstDriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Drivers_SecondDriverId",
                table: "Entries",
                column: "SecondDriverId",
                principalTable: "Drivers",
                principalColumn: "DriverId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
