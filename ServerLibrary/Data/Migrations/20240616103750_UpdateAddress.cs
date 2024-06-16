using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerLibrary.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Addresses_AddressId",
                table: "ApplicationUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "Ward",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ZipCode",
                table: "Addresses",
                newName: "AddressDetail");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "ApplicationUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DistrictId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProvinceId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WardId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdministrativeRegions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeRegions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Provinces",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdministrativeUnitId = table.Column<int>(type: "int", nullable: true),
                    AdministrativeRegionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provinces", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Provinces_AdministrativeRegions_AdministrativeRegionId",
                        column: x => x.AdministrativeRegionId,
                        principalTable: "AdministrativeRegions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Provinces_AdministrativeUnits_AdministrativeUnitId",
                        column: x => x.AdministrativeUnitId,
                        principalTable: "AdministrativeUnits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProvinceCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdministrativeUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Districts_AdministrativeUnits_AdministrativeUnitId",
                        column: x => x.AdministrativeUnitId,
                        principalTable: "AdministrativeUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Districts_Provinces_ProvinceCode",
                        column: x => x.ProvinceCode,
                        principalTable: "Provinces",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AdministrativeUnitId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Wards_AdministrativeUnits_AdministrativeUnitId",
                        column: x => x.AdministrativeUnitId,
                        principalTable: "AdministrativeUnits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Wards_Districts_DistrictCode",
                        column: x => x.DistrictCode,
                        principalTable: "Districts",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_DistrictId",
                table: "Addresses",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ProvinceId",
                table: "Addresses",
                column: "ProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_WardId",
                table: "Addresses",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_AdministrativeUnitId",
                table: "Districts",
                column: "AdministrativeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_ProvinceCode",
                table: "Districts",
                column: "ProvinceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_AdministrativeRegionId",
                table: "Provinces",
                column: "AdministrativeRegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Provinces_AdministrativeUnitId",
                table: "Provinces",
                column: "AdministrativeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_AdministrativeUnitId",
                table: "Wards",
                column: "AdministrativeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_DistrictCode",
                table: "Wards",
                column: "DistrictCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Districts_DistrictId",
                table: "Addresses",
                column: "DistrictId",
                principalTable: "Districts",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Provinces_ProvinceId",
                table: "Addresses",
                column: "ProvinceId",
                principalTable: "Provinces",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Wards_WardId",
                table: "Addresses",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Addresses_AddressId",
                table: "ApplicationUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Districts_DistrictId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Provinces_ProvinceId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Wards_WardId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsers_Addresses_AddressId",
                table: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "Provinces");

            migrationBuilder.DropTable(
                name: "AdministrativeRegions");

            migrationBuilder.DropTable(
                name: "AdministrativeUnits");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_DistrictId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_ProvinceId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_WardId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "ProvinceId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Addresses");

            migrationBuilder.RenameColumn(
                name: "AddressDetail",
                table: "Addresses",
                newName: "ZipCode");

            migrationBuilder.AlterColumn<int>(
                name: "AddressId",
                table: "ApplicationUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ward",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsers_Addresses_AddressId",
                table: "ApplicationUsers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }
    }
}
