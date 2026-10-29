using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VisumCRM.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    IsoCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Flag = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Created = table.Column<long>(type: "bigint", nullable: false),
                    Updated = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    House = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    Door = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: true),
                    PostalzoneId = table.Column<long>(type: "bigint", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    Created = table.Column<long>(type: "bigint", nullable: false),
                    Updated = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Postalzones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Zipcode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    State = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<long>(type: "bigint", nullable: false),
                    Updated = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postalzones", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Postalzones");
        }
    }
}
