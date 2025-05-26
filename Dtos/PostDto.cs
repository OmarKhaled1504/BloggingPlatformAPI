namespace BloggingAPI.Dtos;

public record class PostDto(
    int Id,
    string Title,
    string Content,
    string Category,
    List<string> Tags,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
