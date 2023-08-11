using System.Diagnostics;
using DatabaseLibrary.MsSqlDatabase;
using DatabaseLibrary.MsSqlDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatabaseLibrary_XUnit_Tests.MsSqlDatabase;

public class PostTests
{

  private DbContextOptions<DatabaseContextMsSql> dbContextOptions;

  public PostTests()
  {
    string filePath = @"C:\Users\ciordanidis\Documents\Workspace\Net_Workspace\Guides\DatabaseLibrary_XUnit_Tests\appsettings.json";

    IConfiguration Configuration = new ConfigurationBuilder()
          .SetBasePath(Path.GetDirectoryName(filePath))
          .AddJsonFile("appSettings.json")
          .Build();

    // var dbName = "EF_Test";
    dbContextOptions = new DbContextOptionsBuilder<DatabaseContextMsSql>()
        //.UseInMemoryDatabase(dbName)
        .UseSqlServer(Configuration.GetConnectionString("MsSqlConnection"))
        .EnableSensitiveDataLogging(true)
        .EnableDetailedErrors(true)
        .Options;
  }


  [Fact]
  public void GetListOfPosts()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Post>();
    var result = repository.Filter();

    Assert.True(
      result.Count() > 0,
      $"Expected 0, but got {result.Count()}"
    );
  }

  [Fact]
  public void GetFirstPost()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Post>();
    var result = repository.Filter().FirstOrDefault();

    Assert.True(
        result != null,
        $"Expected {result}, but got null"
      );

    Assert.True(
        result.Title == "PostA_A",
        $"Expected Title = PostA_A, but got {result.Title}"
      );
  }

  //[Fact]
  public void PostAddress()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Post>();
    var result = repository.Create(new Post
    {
      Title = "xUnitTestTitle",
      CreatedBy = "xUnit"
    });

    Assert.True(
      result.Title == "xUnitTestTitle",
      $"Expected xUnitTestTitle, but got {result.Title}"
    );
  }

  //[Fact]
  public void PutAddress()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Post>();
    var result = repository.Filter(a => a.Title == "PostA_A").FirstOrDefault();

    if (result != null)
    {
      result.Title = "PostA_A_Updated";

      var updatedResult = repository.Update(
        a => a.Title == "PostA_A",
        result
      );

      Assert.True(
        updatedResult.Title == "PostA_A_Updated",
        $"Expected Title Updated PostA_A_Updated, but got {updatedResult.Title}"
      );
    }
    else
    {
      Assert.Fail("Post not found");
    }
  }

  //[Fact]
  public void DeleteAddress()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Post>();

    var dtoToDelete = repository.Filter(a => a.Title == "PostA_A_Updated").FirstOrDefault();

    if (dtoToDelete != null)
    {
      var result = repository.Delete(dtoToDelete);

      Assert.True(
        result.Title == "PostA_A_Updated",
        $"Expected Title Updated PostA_A_Updated, but got {result.Title}"
      );
    }
    else
    {
      Assert.Fail("Post not found");
    }
  }

  [Fact]
  public async Task GetListOfPostsAsync()
  {
    var repository = await new TestHelpers(dbContextOptions).CreateRepositoryAsync<Post>();

    var result = await repository.FilterAsync();

    Assert.True(
      result.Count() > 0,
      $"Expected 0, but got {result.Count()}"
    );
  }

  [Fact]
  public async Task GetFirstPostAsync()
  {
    var repository = await new TestHelpers(dbContextOptions).CreateRepositoryAsync<Post>();

    var result = await repository.Filter().FirstOrDefaultAsync();

    Assert.True(
      result != null,
        $"Expected {result}, but got null"
      );

    Assert.True(
      result.Title == "PostA_A",
      $"Expected Title = PostA_A, but got {result.Title}"
    );
  }

}