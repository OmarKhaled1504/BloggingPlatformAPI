using System.ComponentModel.DataAnnotations;

namespace BloggingAPI.Dtos;

public record class PostCreateDto(
    [Required, StringLength(50, MinimumLength = 10)]
    string Title,
    [Required, StringLength(10000, MinimumLength = 20)]
    string Content,
    [Required, StringLength(50, MinimumLength = 3)]
    string Category,
    List<string>? Tags
);
