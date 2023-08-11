using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLibrary.MsSqlDatabase.Migrations
{
  /// <inheritdoc />
  public partial class SeedData : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      var seedDataScript = File.ReadAllText(
        $"{Directory.GetCurrentDirectory()}/MsSqlDatabase/MsSql_Data_Seed.sql"
      );

      migrationBuilder.Sql(seedDataScript);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

    }
  }
}
