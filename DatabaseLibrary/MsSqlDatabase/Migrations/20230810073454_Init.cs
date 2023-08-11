using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLibrary.MsSqlDatabase.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false, comment: "Primary key for blog records.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The url of the blog."),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the user who created the record."),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date and time the record was created."),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog_BlogId", x => x.BlogId);
                },
                comment: "Blogs table.");

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false, comment: "Primary key for post records.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The title of the post."),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The content of the post."),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the user who created the record."),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Date and time the record was created."),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Date and time the record was last updated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_PostId", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_Blog_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blog",
                        principalColumn: "BlogId");
                },
                comment: "Posts table.");

            migrationBuilder.CreateIndex(
                name: "AK_Blog_blogid",
                table: "Blog",
                column: "BlogId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AK_Post_postid",
                table: "Post",
                column: "PostId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_BlogId",
                table: "Post",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Blog");
        }
    }
}
