using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisumCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class addContactpersonEmpliyeeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeerId",
                table: "Contactpersons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SalespersonId",
                table: "Contactpersons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeerId",
                table: "Contactpersons");

            migrationBuilder.DropColumn(
                name: "SalespersonId",
                table: "Contactpersons");
        }
    }
}
