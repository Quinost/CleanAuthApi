using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanAuth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "IsActive", "Password", "Username" },
                values: new object[] { new Guid("aa5d84da-88cd-45a2-b5fb-233fd99b061d"), true, "AQAAAAIAAYagAAAAEJiaHgbU2vnyrKpN1USLPGDxUEDcxp3ylq04xQ9YJx3u2p1EhidfpdqTO97ST0ttvA==", "DefaultUser" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
