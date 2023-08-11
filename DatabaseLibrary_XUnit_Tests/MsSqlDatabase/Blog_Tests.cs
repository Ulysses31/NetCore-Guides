using System.Diagnostics;
using DatabaseLibrary.MsSqlDatabase;
using DatabaseLibrary.MsSqlDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DatabaseLibrary_XUnit_Tests.MsSqlDatabase;

public class BlogTests
{

  private DbContextOptions<DatabaseContextMsSql> dbContextOptions;

  public BlogTests()
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
  public void GetListOfBlogs()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Blog>();
    var result = repository.Filter();

    Assert.True(
      result.Count() > 0,
      $"Expected 0, but got {result.Count()}"
    );
  }

  [Fact]
  public void GetFirstBlog()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Blog>();
    var result = repository.Filter().FirstOrDefault();

    Assert.True(
        result != null,
        $"Expected {result}, but got null"
      );

    Assert.True(
        result.Url == "BlogA",
        $"Expected Url = BlogA, but got {result.Url}"
      );
  }

  //[Fact]
  public void PostAddress()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Blog>();
    var result = repository.Create(new Blog
    {
      Url = "xUnitTestUrl",
      CreatedBy = "xUnit"
    });

    Assert.True(
      result.Url == "xUnitTestUrl",
      $"Expected xUnitTestUrl, but got {result.Url}"
    );
  }

  //[Fact]
  public void PutAddress()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Blog>();
    var result = repository.Filter(a => a.Url == "xUnitTestUrl").FirstOrDefault();

    if (result != null)
    {
      result.Url = "xUnitTestUrl_Updated";

      var updatedResult = repository.Update(
        a => a.Url == "xUnitTestUrl",
        result
      );

      Assert.True(
        updatedResult.Url == "xUnitTestUrl_Updated",
        $"Expected Url Updated xUnitTestUrl_Updated, but got {updatedResult.Url}"
      );
    }
    else
    {
      Assert.Fail("Blog not found");
    }
  }

  //[Fact]
  public void DeleteAddress()
  {
    var repository = new TestHelpers(dbContextOptions).CreateRepository<Blog>();

    var dtoToDelete = repository.Filter(a => a.Url == "xUnitTestUrl_Updated").FirstOrDefault();

    if (dtoToDelete != null)
    {
      var result = repository.Delete(dtoToDelete);

      Assert.True(
        result.Url == "xUnitTestUrl_Updated",
        $"Expected Url Updated xUnitTestUrl_Updated, but got {result.Url}"
      );
    }
    else
    {
      Assert.Fail("Blog not found");
    }
  }

  [Fact]
  public async Task GetListOfBlogsAsync()
  {
    var repository = await new TestHelpers(dbContextOptions).CreateRepositoryAsync<Blog>();

    var result = await repository.FilterAsync();

    Assert.True(
      result.Count() > 0,
      $"Expected 0, but got {result.Count()}"
    );
  }

  [Fact]
  public async Task GetFirstBlogAsync()
  {
    var repository = await new TestHelpers(dbContextOptions).CreateRepositoryAsync<Blog>();

    var result = await repository.Filter().FirstOrDefaultAsync();

    Assert.True(
      result != null,
        $"Expected {result}, but got null"
      );

    Assert.True(
      result.Url == "BlogA",
      $"Expected Url = BlogA, but got {result.Url}"
    );
  }

}