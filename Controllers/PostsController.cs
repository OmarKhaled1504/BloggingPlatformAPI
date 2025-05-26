using BloggingAPI.Data;
using BloggingAPI.Dtos;
using BloggingAPI.Entities;
using BloggingAPI.Mapping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly BlogContext _context;
    public PostsController(BlogContext context)
    {
        _context = context;
    }
    //Post /api/posts
    [HttpPost]
    [ProducesResponseType(typeof(PostDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PostDto>> CreatePost(PostCreateDto dto)
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
        return CreatedAtAction(nameof(GetPost), new { id = post.Id }, savedPost!.ToDto());
    }


    //GET /api/posts/1
    [HttpGet("{id}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        var post = await _context.Posts.Include(post => post.Category).Include(post => post.PostTags).ThenInclude(pt => pt.Tag).SingleOrDefaultAsync(post => post.Id == id);
        return post is null ? NotFound() : Ok(post.ToDto());
    }
}
