using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLibrary.MsSqlDatabase.Migrations
{
  /// <inheritdoc />
  public partial class Functions : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var fnGetYearFromDate = @"
        SET ANSI_NULLS ON
        GO
        SET QUOTED_IDENTIFIER ON
        GO

        CREATE FUNCTION [dbo].[fnGetYearFromDate]
        (
            @date DATETIME
        )
        RETURNS [INT]
        AS
        BEGIN
            RETURN YEAR(@date)
        END
      ";

      migrationBuilder.Sql(fnGetYearFromDate);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
