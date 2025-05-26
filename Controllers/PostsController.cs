using BloggingAPI.Data;
using BloggingAPI.Dtos;
using BloggingAPI.Entities;
using BloggingAPI.Mapping;
using BloggingAPI.Responses;
using BloggingAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    public PostsController(IPostService postService)
    {
        _postService = postService;
    }
    //Post /api/posts
    [HttpPost]
    [ProducesResponseType(typeof(PostDto), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PostDto>> CreatePost(PostCreateDto dto)
    {

        var newPost = await _postService.CreatePostAsync(dto);
        return CreatedAtAction(nameof(GetPost), new { id = newPost.Id }, newPost);

    }

    //GET /api/posts/1
    [HttpGet("{id}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(PostDto), 200)]
    public async Task<ActionResult<PostDto>> GetPost(int id)
    {
        var post = await _postService.GetPostByIdAsync(id);
        return post is null ? NotFound() : Ok(post);
    }

    //GET /api/posts
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<PostDto>), 200)]
    public async Task<ActionResult<IEnumerable<PostDto>>> GetPosts(int pageNumber = 1, int pageSize = 10, string? term = null)
    {
        if (pageNumber < 1 || pageSize < 1)
            return BadRequest("Page number and page size must be positive.");
        return Ok(await _postService.GetPostsAsync(pageNumber, pageSize, term));
    }

    //PUT /api/posts/1
    [HttpPut("{id}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(typeof(PostDto), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<PostDto>> UpdatePost(int id, PostCreateDto dto)
    {
        var response = await _postService.UpdatePostAsync(id, dto);
        if (response is null) return NotFound();
        return Ok(response);
    }

    //DELETE /api/posts/1
    [HttpDelete("{id}")]
    [ProducesResponseType(404)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _postService.DeletePostAsync(id);
        return response ? NoContent() : NotFound();
    }
}
