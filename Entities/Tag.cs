using System;

namespace BloggingAPI.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<PostTag> PostTags { get; set; } = new();
}
