using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Content.Contract.Posts.Models;
using Qwitter.Core.Application.Persistence;
using Qwitter.Core.Application.RestApiClient;

namespace Qwitter.Content.Contract;

[ApiHost("5003", "posts")]
public interface IPostsController
{
    [Authorize]
    [HttpPost]
    Task<CreatePostResponse> CreatePost(CreatePostRequest request);

    [HttpGet("user/{userId}")]
    Task<IEnumerable<PostResponse>> GetUserPosts(Guid userId);

    [HttpPost("latest")]
    Task<IEnumerable<PostResponse>> GetLatestPosts(PaginationRequest request);

    [Authorize]
    [HttpGet("mine")]
    Task<IEnumerable<PostResponse>> GetUsersPosts();

    [Authorize]
    [HttpPost("like")]
    Task<IActionResult> LikePost(LikeDislikeRequest request);

    [Authorize]
    [HttpPost("dislike")]
    Task<IActionResult> DislikePost(LikeDislikeRequest request);
}