using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScissorLink.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShortLink",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Token = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OriginLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPublish = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EditAdminID = table.Column<int>(type: "int", nullable: true),
                    EditAdminDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateAdminID = table.Column<int>(type: "int", nullable: false),
                    CreateAdminDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortLink", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ShortLinkDetail",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortLinkID = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OS = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortLinkDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ShortLinkDetail_ShortLink_ShortLinkID",
                        column: x => x.ShortLinkID,
                        principalTable: "ShortLink",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShortLink_Token",
                table: "ShortLink",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortLinkDetail_ShortLinkID",
                table: "ShortLinkDetail",
                column: "ShortLinkID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShortLinkDetail");

            migrationBuilder.DropTable(
                name: "ShortLink");
        }
    }
}
