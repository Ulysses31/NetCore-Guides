
using DatabaseLibrary.MsSqlDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DatabaseLibrary.MsSqlDatabase
{
  public partial class DatabaseContextMsSql : DbContext
  {

    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<Post> Posts { get; set; }

    protected DatabaseContextMsSql()
    {
    }

    public DatabaseContextMsSql(
      DbContextOptions<DatabaseContextMsSql> options
    ) : base(options)
    {
      ChangeTracker.StateChanged += UpdateTimestamps;
      ChangeTracker.Tracked += UpdateTimestamps;
    }

    protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder
    )
    {
      base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder
    )
    {
      base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
      modelBuilder.Entity<Blog>(entity =>
      {
        entity.HasKey(key => key.BlogId).HasName("PK_Blog_BlogId");
        entity.ToTable("Blog", tb => tb.HasComment("Blogs table."));
        entity.HasIndex(idx => idx.BlogId, "AK_Blog_blogid").IsUnique();

        entity.Property(p => p.BlogId)
          .HasComment("Primary key for blog records.")
          .HasColumnName("BlogId");

        entity.Property(p => p.Url)
          .HasComment("The url of the blog.")
          .HasColumnName("Url")
          .IsRequired(true);

        entity.Property(p => p.CreatedBy)
          .HasMaxLength(100)
          .HasComment("Name of the user who created the record.")
          .IsRequired(true);

        entity.Property(p => p.CreatedDate)
          .HasComment("Date and time the record was created.")
          .HasColumnType("datetime")
          .IsRequired(true);

        entity.Property(p => p.ModifiedDate)
          .HasComment("Date and time the record was last updated.")
          .HasColumnType("datetime");

        entity.HasMany(d => d.Posts).WithOne(d => d.Blog)
          .HasForeignKey(d => d.BlogId)
          .IsRequired()
          .OnDelete(DeleteBehavior.ClientSetNull);
      });

      modelBuilder.Entity<Post>(entity =>
      {
        entity.HasKey(key => key.PostId).HasName("PK_Post_PostId");
        entity.ToTable("Post", tb => tb.HasComment("Posts table."));
        entity.HasIndex(idx => idx.PostId, "AK_Post_postid").IsUnique();

        entity.Property(p => p.PostId)
          .HasComment("Primary key for post records.")
          .HasColumnName("PostId");

        entity.Property(p => p.Title)
          .HasComment("The title of the post.")
          .HasColumnName("Title")
          .IsRequired(true);

        entity.Property(p => p.Content)
          .HasComment("The content of the post.")
          .HasColumnName("Content")
          .IsRequired(true);

        entity.Property(p => p.CreatedBy)
          .HasMaxLength(100)
          .HasComment("Name of the user who created the record.")
          .IsRequired(true);

        entity.Property(p => p.CreatedDate)
          .HasComment("Date and time the record was created.")
          .HasColumnType("datetime")
          .IsRequired(true);

        entity.Property(p => p.ModifiedDate)
          .HasComment("Date and time the record was last updated.")
          .HasColumnType("datetime");
      });

      base.OnModelCreating(modelBuilder);
    }

    private void UpdateTimestamps(object sender, EntityEntryEventArgs e)
    {
      if (e.Entry.Entity is BaseEntity entityWithTimestamps)
      {
        switch (e.Entry.State)
        {
          // case EntityState.Deleted:
          // entityWithTimestamps.Deleted = DateTime.UtcNow;
          //    Console.WriteLine($"Stamped for delete: {e.Entry.Entity}");
          //    break;
          case EntityState.Modified:
            entityWithTimestamps.ModifiedDate = DateTime.UtcNow;
            Console.WriteLine($"Stamped for update: {e.Entry.Entity}");
            break;
          case EntityState.Added:
            entityWithTimestamps.CreatedDate = DateTime.UtcNow;
            Console.WriteLine($"Stamped for insert: {e.Entry.Entity}");
            break;
        }
      }
    }
  }

  public static class ISserviceCollectionExtension
  {
    public static IServiceCollection configureservice(
      this IServiceCollection service,
      IConfiguration Configuration
    )
    {
      string filePath = @"C:\Users\ciordanidis\Documents\Workspace\Net_Workspace\Guides\DatabaseLibrary\appsettings.json";

      Configuration = new ConfigurationBuilder()
          .SetBasePath(Path.GetDirectoryName(filePath))
          .AddJsonFile("appSettings.json")
          .Build();

      service.AddDbContext<DatabaseContextMsSql>(options =>
          // options.UseInMemoryDatabase("EF_TEST"));
          options
            .UseSqlServer(Configuration.GetConnectionString("MsSqlConnection"))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
      );

      return service;
    }
  }

  public class AppDbContextFactory : IDesignTimeDbContextFactory<DatabaseContextMsSql>
  {
    private readonly IConfiguration Configuration;

    public AppDbContextFactory()
    {
    }

    public AppDbContextFactory(
      IConfiguration configuration
    )
    {
      Configuration = configuration;
    }

    public DatabaseContextMsSql CreateDbContext(string[] args)
    {
      // string filePath = @"C:\Users\Administrator\source\repos\CheckPoint\NullMVC\appsettings.json";
      string filePath = @"C:\Users\ciordanidis\Documents\Workspace\Net_Workspace\Guides\DatabaseLibrary\appsettings.json";

      IConfiguration Configuration = new ConfigurationBuilder()
          .SetBasePath(Path.GetDirectoryName(filePath))
          .AddJsonFile("appSettings.json")
          .Build();

      var optionsBuilder = new DbContextOptionsBuilder<DatabaseContextMsSql>();
      optionsBuilder.EnableDetailedErrors();
      optionsBuilder.EnableSensitiveDataLogging();
      optionsBuilder.LogTo(Console.WriteLine);

      // optionsBuilder.UseMySQL(Configuration.GetConnectionString("MySqlConnection"));
      optionsBuilder.UseSqlServer(Configuration.GetConnectionString("MsSqlConnection"));

      return new DatabaseContextMsSql(optionsBuilder.Options);
    }
  }

}