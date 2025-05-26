using BloggingAPI.Data;
using BloggingAPI.Dtos;
using BloggingAPI.Entities;
using BloggingAPI.Mapping;
using BloggingAPI.Responses;
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

    //GET /api/posts
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PostDto>), 200)]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(int pageNumber = 1, int pageSize = 10, string? term = null)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be positive.");
        }
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
        return Ok(new PagedResponse<PostDto>
        {
            Data = posts,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    //PUT /api/posts/1
    [HttpPut("{id}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(PostDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, PostCreateDto dto)
    {
        var post = await _context.Posts.Include(post => post.Category).Include(post => post.PostTags).ThenInclude(pt => pt.Tag).SingleOrDefaultAsync(post => post.Id == id);
        if (post is null) return NotFound();

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

        return Ok(post.ToDto());
    }

    //DELETE /api/posts/1
    [HttpDelete("{id}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post is null) return NotFound();

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
