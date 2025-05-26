using BloggingAPI.Dtos;
using BloggingAPI.Responses;

namespace BloggingAPI.Services;

public interface IPostService
{
    Task<PostDto> CreatePostAsync(PostCreateDto dto);
    Task<PostDto?> GetPostByIdAsync(int id);
    Task<PagedResponse<PostDto>> GetPostsAsync(int pageNumber, int pageSize, string? term);
    Task<bool> DeletePostAsync(int id);
    Task<PostDto?> UpdatePostAsync(int id, PostCreateDto dto);
}
