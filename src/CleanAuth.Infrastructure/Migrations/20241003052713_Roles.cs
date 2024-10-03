using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("f3f533fd-41c4-44f1-a67e-8062e6d207a6"), "Admin" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aa5d84da-88cd-45a2-b5fb-233fd99b061d"),
                columns: new[] { "Password", "RoleId", "Username" },
                values: new object[] { "AQAAAAIAAYagAAAAEHaohRXUvti7bHQgzukSyRT5TMSU4Jd2JObtiWQdC7bHAFod/n1z212tOZcVSmbc6A==", new Guid("f3f533fd-41c4-44f1-a67e-8062e6d207a6"), "AdminUser" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("aa5d84da-88cd-45a2-b5fb-233fd99b061d"),
                columns: new[] { "Password", "Username" },
                values: new object[] { "AQAAAAIAAYagAAAAEJiaHgbU2vnyrKpN1USLPGDxUEDcxp3ylq04xQ9YJx3u2p1EhidfpdqTO97ST0ttvA==", "DefaultUser" });
        }
    }
}
