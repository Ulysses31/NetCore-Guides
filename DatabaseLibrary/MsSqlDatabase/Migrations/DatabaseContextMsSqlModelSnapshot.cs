﻿// <auto-generated />
using System;
using DatabaseLibrary.MsSqlDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DatabaseLibrary.MsSqlDatabase.Migrations
{
    [DbContext(typeof(DatabaseContextMsSql))]
    partial class DatabaseContextMsSqlModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DatabaseLibrary.MsSqlDatabase.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BlogId")
                        .HasComment("Primary key for blog records.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogId"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name of the user who created the record.");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was created.");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was last updated.");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Url")
                        .HasComment("The url of the blog.");

                    b.HasKey("BlogId")
                        .HasName("PK_Blog_BlogId");

                    b.HasIndex(new[] { "BlogId" }, "AK_Blog_blogid")
                        .IsUnique();

                    b.ToTable("Blog", null, t =>
                        {
                            t.HasComment("Blogs table.");
                        });
                });

            modelBuilder.Entity("DatabaseLibrary.MsSqlDatabase.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("PostId")
                        .HasComment("Primary key for post records.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PostId"));

                    b.Property<int>("BlogId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Content")
                        .HasComment("The content of the post.");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name of the user who created the record.");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was created.");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime")
                        .HasComment("Date and time the record was last updated.");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Title")
                        .HasComment("The title of the post.");

                    b.HasKey("PostId")
                        .HasName("PK_Post_PostId");

                    b.HasIndex("BlogId");

                    b.HasIndex(new[] { "PostId" }, "AK_Post_postid")
                        .IsUnique();

                    b.ToTable("Post", null, t =>
                        {
                            t.HasComment("Posts table.");
                        });
                });

            modelBuilder.Entity("DatabaseLibrary.MsSqlDatabase.Models.Post", b =>
                {
                    b.HasOne("DatabaseLibrary.MsSqlDatabase.Models.Blog", "Blog")
                        .WithMany("Posts")
                        .HasForeignKey("BlogId")
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("DatabaseLibrary.MsSqlDatabase.Models.Blog", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
