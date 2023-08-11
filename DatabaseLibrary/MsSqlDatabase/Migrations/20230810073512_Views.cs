using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLibrary.MsSqlDatabase.Migrations
{
  /// <inheritdoc />
  public partial class Views : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var vBlogsDetailed = @"
        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO

        CREATE VIEW [dbo].[vBlogsDetailed]
        AS
            SELECT 
                dbo.Blog.BlogId, 
                dbo.Blog.Url, 
                dbo.Blog.CreatedDate AS Blog_CreatedDate, 
                dbo.Post.PostId, 
                dbo.Post.Title, 
                dbo.Post.[Content], 
                dbo.Post.BlogId AS Post_BlogId, 
                dbo.Post.CreatedDate AS Post_CreatedDate
            FROM dbo.Blog 
            INNER JOIN dbo.Post 
                ON dbo.Blog.BlogId = dbo.Post.BlogId
        GO
        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Blogs detailed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vBlogsDetailed'
        GO
      ";
      migrationBuilder.Sql(vBlogsDetailed);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
