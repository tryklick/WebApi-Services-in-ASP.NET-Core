using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class initial_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Seeded = table.Column<bool>(type: "bit", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                });

            migrationBuilder.CreateTable(
                name: "Destinations",
                columns: table => new
                {
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ZipCode = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinations", x => x.DestinationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Sightseeings",
                columns: table => new
                {
                    SightseeingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Seeded = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Sightnames = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sightseeings", x => x.SightseeingId);
                    table.ForeignKey(
                        name: "FK_Sightseeings_Destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "Destinations",
                        principalColumn: "DestinationId");
                });

            migrationBuilder.CreateTable(
                name: "CommentUser",
                columns: table => new
                {
                    CommentsCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUser", x => new { x.CommentsCommentId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_CommentUser_Comments_CommentsCommentId",
                        column: x => x.CommentsCommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentSightseeing",
                columns: table => new
                {
                    CommentsCommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SightseeingsSightseeingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentSightseeing", x => new { x.CommentsCommentId, x.SightseeingsSightseeingId });
                    table.ForeignKey(
                        name: "FK_CommentSightseeing_Comments_CommentsCommentId",
                        column: x => x.CommentsCommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentSightseeing_Sightseeings_SightseeingsSightseeingId",
                        column: x => x.SightseeingsSightseeingId,
                        principalTable: "Sightseeings",
                        principalColumn: "SightseeingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentSightseeing_SightseeingsSightseeingId",
                table: "CommentSightseeing",
                column: "SightseeingsSightseeingId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentUser_UsersUserId",
                table: "CommentUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sightseeings_DestinationId",
                table: "Sightseeings",
                column: "DestinationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentSightseeing");

            migrationBuilder.DropTable(
                name: "CommentUser");

            migrationBuilder.DropTable(
                name: "Sightseeings");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Destinations");
        }
    }
}
