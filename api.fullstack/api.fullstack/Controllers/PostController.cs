using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using api.fullstack.Model;
using api.fullstack.Dto;

namespace api.fullstack.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PostController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<PostDto>>> GetPosts([FromQuery] string? q = null)
    {
        try
        {
            // Get HttpClient from factory
            HttpClient client = _httpClientFactory.CreateClient();
            string url = "https://jsonplaceholder.typicode.com/posts";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string jsonContent = await response.Content.ReadAsStringAsync();

            // Deserialize JSON into C# objects
            List<Post> posts = JsonSerializer.Deserialize<List<Post>>(jsonContent);

            if (string.IsNullOrWhiteSpace(q))
            {
                // If no search term, return all posts grouped by user
                var groupedByUser = posts
                    .GroupBy(p => p.userId)
                    .Select(g => new UserPostsDto { UserId = g.Key, Posts = g.Select(post => new PostDto { Id = post.id, Title = post.title, Body = post.body }).ToList() })
                    .ToList();

                return Ok(groupedByUser);
            }

            // Filter posts by title matching search term (case-insensitive)
            var filteredPosts = posts
                .Where(p => p.title.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // Group filtered posts by user
            var groupedResults = filteredPosts
                .GroupBy(p => p.userId)
                .Select(g => new UserPostsDto { UserId = g.Key, Posts = g.Select(post => new PostDto { Id = post.id, Title = post.title, Body = post.body }).ToList() })
                .ToList();

            return Ok(groupedResults);
        }
        catch (Exception ex)
        {
            // Handle the exception and return an appropriate response
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}