using System;
using BloggingAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BloggingAPI.Data;

public class BlogContext : DbContext
{
    public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PostTag> PostTags => Set<PostTag>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Composite key for PostTag
    modelBuilder.Entity<PostTag>()
    .HasKey(pt => new { pt.PostId, pt.TagId });

modelBuilder.Entity<PostTag>()
    .HasOne(pt => pt.Post)
    .WithMany(p => p.PostTags)
    .HasForeignKey(pt => pt.PostId)
    .OnDelete(DeleteBehavior.Cascade); // ✅ Delete relationship if Post is deleted

modelBuilder.Entity<PostTag>()
    .HasOne(pt => pt.Tag)
    .WithMany(t => t.PostTags)
    .HasForeignKey(pt => pt.TagId)
    .OnDelete(DeleteBehavior.Cascade); // ✅ Delete relationship if Tag is deleted

}
}
