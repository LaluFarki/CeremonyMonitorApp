using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CeremonyMonitorApp.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employess_Departments_DepartmentId",
                table: "Employess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employess",
                table: "Employess");

            migrationBuilder.RenameTable(
                name: "Employess",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_Employess_DepartmentId",
                table: "Employees",
                newName: "IX_Employees_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "ExternalId", "LastSyncedAt", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Air Conditioner" },
                    { 2, null, null, "Refrigeration" },
                    { 3, null, null, "Home Appliances" },
                    { 4, null, null, "TV Assembly" },
                    { 5, null, null, "Battery Pack" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "ExternalID", "FullName", "LastSyncedAt", "Position" },
                values: new object[,]
                {
                    { 1, 1, null, "Takeshi Sato", null, "Production Manager" },
                    { 2, 1, null, "Maya Nakajima", null, "Executive Assistant" },
                    { 3, 1, null, "Ryu Watanabe", null, "Internal Comms Specialist" },
                    { 4, 2, null, "Dewi Anjani", null, "HR Coordinator" },
                    { 5, 2, null, "Hiroshi Kato", null, "Line Lead" },
                    { 6, 3, null, "Airi Kobayashi", null, "Quality Inspector" },
                    { 7, 3, null, "Budi Santoso", null, "Shift Supervisor" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employess");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employess",
                newName: "IX_Employess_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employess",
                table: "Employess",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employess_Departments_DepartmentId",
                table: "Employess",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
