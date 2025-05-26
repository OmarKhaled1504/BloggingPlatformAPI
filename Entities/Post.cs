using System;

namespace BloggingAPI.Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Category? Category { get; set; }
    public int CategoryId { get; set; } //Foreign key
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; } = null;
    public List<PostTag> PostTags { get; set; }  = new();
}
