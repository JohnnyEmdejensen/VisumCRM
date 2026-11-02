using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisumCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class addCompanySalespersonId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SalespersonId",
                table: "Companies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalespersonId",
                table: "Companies");
        }
    }
}
