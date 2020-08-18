using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace mdijk.f1.domain.Migrations
{
    public partial class index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RaceEvents_SeasonId",
                table: "RaceEvents");

            migrationBuilder.DropIndex(
                name: "IX_EventResults_RaceEventId",
                table: "EventResults");

            migrationBuilder.DropIndex(
                name: "IX_EventResultMetaDatas_EventResultId",
                table: "EventResultMetaDatas");

            migrationBuilder.DropIndex(
                name: "IX_DriverEntries_EntryId",
                table: "DriverEntries");

            migrationBuilder.CreateIndex(
                name: "IX_RaceEvents_SeasonId_CircuitId",
                table: "RaceEvents",
                columns: new[] { "SeasonId", "CircuitId" })
                .Annotation("SqlServer:Include", new[] { "RaceEventId", "SequenceNumber", "RaceEventName" });

            migrationBuilder.CreateIndex(
                name: "IX_EventResults_WithIncludes",
                table: "EventResults",
                columns: new[] { "RaceEventId", "EventResultId", "DriverEntryId" })
                .Annotation("SqlServer:Include", new[] { "FinishingPosition", "FastestLap" });

            migrationBuilder.CreateIndex(
                name: "IX_EventResultMetaDatas_EventResultId_EventResultMetaDataId",
                table: "EventResultMetaDatas",
                columns: new[] { "EventResultId", "EventResultMetaDataId" })
                .Annotation("SqlServer:Include", new[] { "Key", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_DriverEntries_EntryId_DriverId",
                table: "DriverEntries",
                columns: new[] { "EntryId", "DriverId" })
                .Annotation("SqlServer:Include", new[] { "DriverEntryId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RaceEvents_SeasonId_CircuitId",
                table: "RaceEvents");

            migrationBuilder.DropIndex(
                name: "IX_EventResults_WithIncludes",
                table: "EventResults");

            migrationBuilder.DropIndex(
                name: "IX_EventResultMetaDatas_EventResultId_EventResultMetaDataId",
                table: "EventResultMetaDatas");

            migrationBuilder.DropIndex(
                name: "IX_DriverEntries_EntryId_DriverId",
                table: "DriverEntries");

            migrationBuilder.CreateIndex(
                name: "IX_RaceEvents_SeasonId",
                table: "RaceEvents",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_EventResults_RaceEventId",
                table: "EventResults",
                column: "RaceEventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventResultMetaDatas_EventResultId",
                table: "EventResultMetaDatas",
                column: "EventResultId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverEntries_EntryId",
                table: "DriverEntries",
                column: "EntryId");
        }
    }
}
