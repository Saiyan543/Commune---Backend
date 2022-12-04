using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Homestead.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34528ca8-094d-4659-9099-1f84ba3dc0ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "87ca35e0-ccbc-4133-ad2a-89ef2576472b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b85da541-637c-4866-a64d-7e559f125c2e");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "34528ca8-094d-4659-9099-1f84ba3dc0ef", "fee87fc1-d92a-4bde-b4e2-7a27fb6ceab7", "Company", "COMPANY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "87ca35e0-ccbc-4133-ad2a-89ef2576472b", "b1a4d6bc-e7dd-46de-8d83-ca24d64426ce", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b85da541-637c-4866-a64d-7e559f125c2e", "121889fa-4622-4e03-aa9e-23ab8bcb64f6", "Security", "SECURITY" });
        }
    }
}