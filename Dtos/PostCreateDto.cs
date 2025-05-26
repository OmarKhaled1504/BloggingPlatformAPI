namespace BloggingAPI.Dtos;

public record class PostCreateDto(
    string Title,
    string Content,
    string Category,
    List<string>? Tags
);
