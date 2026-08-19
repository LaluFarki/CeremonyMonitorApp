using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CeremonyMonitorApp.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeCeremonyChildren : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MCChecklistItems_Ceremonies_CeremonyId",
                table: "MCChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PrayerTexts_Ceremonies_CeremonyId",
                table: "PrayerTexts");

            migrationBuilder.AddForeignKey(
                name: "FK_MCChecklistItems_Ceremonies_CeremonyId",
                table: "MCChecklistItems",
                column: "CeremonyId",
                principalTable: "Ceremonies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrayerTexts_Ceremonies_CeremonyId",
                table: "PrayerTexts",
                column: "CeremonyId",
                principalTable: "Ceremonies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MCChecklistItems_Ceremonies_CeremonyId",
                table: "MCChecklistItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PrayerTexts_Ceremonies_CeremonyId",
                table: "PrayerTexts");

            migrationBuilder.AddForeignKey(
                name: "FK_MCChecklistItems_Ceremonies_CeremonyId",
                table: "MCChecklistItems",
                column: "CeremonyId",
                principalTable: "Ceremonies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrayerTexts_Ceremonies_CeremonyId",
                table: "PrayerTexts",
                column: "CeremonyId",
                principalTable: "Ceremonies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
