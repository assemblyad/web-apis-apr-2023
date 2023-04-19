using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrApi.Migrations
{
    /// <inheritdoc />
    public partial class SalaryForHiringRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "HiringRequests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salary",
                table: "HiringRequests");
        }
    }
}
