using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisumCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class adCompanyFounded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Employees",
                table: "Companies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Founded",
                table: "Companies",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employees",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Founded",
                table: "Companies");
        }
    }
}
