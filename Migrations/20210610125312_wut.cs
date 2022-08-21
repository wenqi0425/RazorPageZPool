using Microsoft.EntityFrameworkCore.Migrations;

namespace ZPool.Migrations
{
    public partial class wut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "2dfa2d17-9500-4ba6-b017-0d9d9fbbef12");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2782c9cb-c0a4-43c8-a1cd-3a70e1aee57d", "AQAAAAEAACcQAAAAEGw+Cbn7noTwJqbBHaAEuDpGl6ZIjRVNXjbI62rMXC/OHz8034uzd0SWeAWM0zBq6g==", "1a4f0bc5-c001-4164-a95f-4d62ac7d0a2a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e110f032-daeb-4ce0-88d9-a0defb76aaec");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "162170a3-dbd2-492e-b058-61fd3deda979", "AQAAAAEAACcQAAAAEL7BlCIzrBycQEvM+673MWf8p8wamEr/76+B9fVWgAwiK4eLIQ+XZRZxk1eIcDr83Q==", "164dc7c6-865c-4605-aa44-ea66d11478cb" });
        }
    }
}
