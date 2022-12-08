using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homestead.Migrations
{
    public partial class fixRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3004ed71-43f8-40f3-be91-260a386ac6e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "396fc235-737b-4ec0-ab5b-0d25234bf513");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5698f7b5-0306-48e4-841b-2504e8662e29");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0b38c7cf-f3ba-4cd5-945c-d0e425a77740", "6ffda52d-278c-422d-ac12-8fa1abd0dccc", "Security", "SECURITY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "aabe682e-5ee0-4f2c-add7-8f6309795a58", "b011d71a-775d-4e0e-ae4f-25f5487f3548", "Club", "CLUB" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d122d7ad-1c96-43f8-98ba-cb1999250044", "af14ad72-eadd-466b-93e4-936520185121", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0b38c7cf-f3ba-4cd5-945c-d0e425a77740");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aabe682e-5ee0-4f2c-add7-8f6309795a58");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d122d7ad-1c96-43f8-98ba-cb1999250044");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3004ed71-43f8-40f3-be91-260a386ac6e5", "90950605-798f-4dfd-abc9-51d3dc107d91", "Security", "SECURITY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "396fc235-737b-4ec0-ab5b-0d25234bf513", "353b7c30-3a40-4475-b8ea-281bf846186f", "Company", "COMPANY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5698f7b5-0306-48e4-841b-2504e8662e29", "abf0a091-590e-46bb-a6e5-c71b2b5ede13", "Administrator", "ADMINISTRATOR" });
        }
    }
}