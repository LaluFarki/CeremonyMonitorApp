using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CeremonyMonitorApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAwardee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Awardees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CeremonyId = table.Column<int>(type: "int", nullable: false),
                    NominatingDepartmentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedById = table.Column<int>(type: "int", nullable: false),
                    Stage = table.Column<int>(type: "int", nullable: false),
                    HrAdminNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedByHrAdminId = table.Column<int>(type: "int", nullable: true),
                    HrManagerNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedByHrManagerId = table.Column<int>(type: "int", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Awardees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Awardees_AppUsers_ReviewedByHrAdminId",
                        column: x => x.ReviewedByHrAdminId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Awardees_AppUsers_ReviewedByHrManagerId",
                        column: x => x.ReviewedByHrManagerId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Awardees_AppUsers_SubmittedById",
                        column: x => x.SubmittedById,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Awardees_Ceremonies_CeremonyId",
                        column: x => x.CeremonyId,
                        principalTable: "Ceremonies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Awardees_Departments_NominatingDepartmentId",
                        column: x => x.NominatingDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Awardees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Awardees_CeremonyId",
                table: "Awardees",
                column: "CeremonyId");

            migrationBuilder.CreateIndex(
                name: "IX_Awardees_EmployeeId",
                table: "Awardees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Awardees_NominatingDepartmentId",
                table: "Awardees",
                column: "NominatingDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Awardees_ReviewedByHrAdminId",
                table: "Awardees",
                column: "ReviewedByHrAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Awardees_ReviewedByHrManagerId",
                table: "Awardees",
                column: "ReviewedByHrManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Awardees_SubmittedById",
                table: "Awardees",
                column: "SubmittedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Awardees");
        }
    }
}
