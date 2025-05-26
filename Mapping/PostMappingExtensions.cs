using System;
using BloggingAPI.Dtos;
using BloggingAPI.Entities;

namespace BloggingAPI.Mapping;

public static class PostMappingExtensions
{
    public static PostDto ToDto(this Post post)
    {
        return new PostDto(
            post.Id,
            post.Title,
            post.Content,
            post.Category!.Name,
            post.PostTags.Select(postTag => postTag.Tag.Name).ToList(),
            post.CreatedAt,
            post.UpdatedAt
        );
    }
    public static Post ToEntity(this PostCreateDto dto)
    {
        return new Post
        {
            Title = dto.Title,
            Content = dto.Content
        };
    }
}
