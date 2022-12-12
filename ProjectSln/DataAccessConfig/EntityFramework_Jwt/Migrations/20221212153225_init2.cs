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
                values: new object[] { "2495b829-59a2-4225-bd06-2d07ea5b6f1b", "e54cb830-40af-4b79-81f4-5deab01a725b", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3e5c678f-60fb-4773-bdc6-32a311b3c463", "b9bc2b69-1b4f-4e2b-9d9a-cff624383daf", "Security", "SECURITY" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ec1d9a61-20cb-4ef8-9a23-3ba3b1111557", "b4dde19e-365c-4641-b41b-e9144ab7f43d", "Club", "CLUB" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2495b829-59a2-4225-bd06-2d07ea5b6f1b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3e5c678f-60fb-4773-bdc6-32a311b3c463");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ec1d9a61-20cb-4ef8-9a23-3ba3b1111557");

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
    }
}
