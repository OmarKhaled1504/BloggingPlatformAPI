using System;
using BloggingAPI.Data;
using BloggingAPI.Dtos;
using BloggingAPI.Entities;
using BloggingAPI.Mapping;
using BloggingAPI.Responses;
using Microsoft.EntityFrameworkCore;

namespace BloggingAPI.Services;

public class PostService : IPostService
{
    private readonly BlogContext _context;
    public PostService(BlogContext context)
    {
        _context = context;
    }

    public async Task<PostDto> CreatePostAsync(PostCreateDto dto)
    {
        var post = dto.ToEntity();
        var cat = await _context.Categories.FirstOrDefaultAsync(cat => cat.Name == dto.Category);
        if (cat == null)
        {
            cat = new Category { Name = dto.Category };
            _context.Categories.Add(cat);
        }
        post.Category = cat;
        if (dto.Tags?.Count > 0)
        {
            foreach (string tagName in dto.Tags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(tag => tag.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                }

                post.PostTags.Add(new PostTag
                {
                    Post = post,
                    Tag = tag!
                });

            }
        }
        post.CreatedAt = DateTime.UtcNow;

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        var savedPost = _context.Posts
        .Include(saved => saved.Category)
        .Include(saved => saved.PostTags)
        .ThenInclude(pt => pt.Tag)
        .FirstOrDefault(saved => saved.Id == post.Id);
        return savedPost!.ToDto();
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post is null) return false;

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<PostDto?> GetPostByIdAsync(int id)
    {
        var post = await _context.Posts
        .Include(post => post.Category)
        .Include(post => post.PostTags)
        .ThenInclude(pt => pt.Tag)
        .SingleOrDefaultAsync(post => post.Id == id);
        return post is null ? null : post.ToDto();
    }

    public async Task<PagedResponse<PostDto>> GetPostsAsync(int pageNumber, int pageSize, string? term)
    {
        if (pageSize > 100)
            pageSize = 100;

        var query = _context.Posts
        .Include(post => post.Category)
        .Include(post => post.PostTags)
        .ThenInclude(pt => pt.Tag)
        .AsQueryable();

        if (!string.IsNullOrEmpty(term))
        {
            query = query
            .Where(post => post.Title.Contains(term) || post.Content.Contains(term) || post.Category!.Name.Contains(term));
        }
        var totalCount = await query.CountAsync();
        var posts = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(post => post.ToDto())
        .ToListAsync();
        return new PagedResponse<PostDto>
        {
            Data = posts,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<PostDto?> UpdatePostAsync(int id, PostCreateDto dto)
    {
        var post = await _context.Posts.Include(post => post.Category).Include(post => post.PostTags).ThenInclude(pt => pt.Tag).SingleOrDefaultAsync(post => post.Id == id);
        if (post is null) return null;

        post.Title = dto.Title;
        post.Content = dto.Content;


        var cat = await _context.Categories.FirstOrDefaultAsync(cat => cat.Name == dto.Category);
        if (cat == null)
        {
            cat = new Category { Name = dto.Category };
            _context.Categories.Add(cat);
        }
        post.Category = cat;

        _context.PostTags.RemoveRange(post.PostTags);
        post.PostTags.Clear();
        if (dto.Tags?.Count > 0)
        {

            foreach (string tagName in dto.Tags)
            {
                var tag = await _context.Tags.FirstOrDefaultAsync(tag => tag.Name == tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    _context.Tags.Add(tag);
                }

                post.PostTags.Add(new PostTag
                {
                    Post = post,
                    Tag = tag!
                });
            }
        }
        post.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return post.ToDto();

    }
}
