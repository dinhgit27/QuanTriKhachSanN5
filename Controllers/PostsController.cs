using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanTriKhachSanN5.DTOs.Post;
using QuanTriKhachSanN5.Interfaces;

namespace QuanTriKhachSanN5.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ManagePosts")]
public class PostsController : ControllerBase
{

    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDTO>>> GetAll()
    {
        var posts = await _postService.GetAllAsync();
        return Ok(posts);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDTO>> GetById(int id)
    {
        var post = await _postService.GetByIdAsync(id);
        if (post is null)
            return NotFound();

        return Ok(post);
    }

    [HttpPost]
    public async Task<ActionResult<PostDTO>> Create([FromBody] CreatePostDTO dto)
    {
        var created = await _postService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreatePostDTO dto)
    {
        var updated = await _postService.UpdateAsync(id, dto);
        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _postService.DeleteAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
