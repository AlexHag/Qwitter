using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Content.Contract;
using Qwitter.Content.Contract.Posts.Models;
using Qwitter.Content.Posts.Repositories;
using Qwitter.Core.Application.Authentication;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Kafka;

namespace Qwitter.Content.Posts;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase, IPostsController
{
    private readonly IMapper _mapper;
    private readonly IPostsRepository _postsRepository;
    private readonly IEventProducer _eventProduer;

    public PostsController(
        IMapper mapper,
        IPostsRepository postsRepository,
        IEventProducer eventProducer)
    {
        _mapper = mapper;
        _postsRepository = postsRepository;
        _eventProduer = eventProducer;
    }

    [HttpPost]
    [Authorize]
    public async Task<CreatePostResponse> CreatePost(CreatePostRequest request)
    {
        if (string.IsNullOrEmpty(request.Content))
        {
            throw new BadRequestApiException("Content is required");
        }

        var post = await _postsRepository.CreatePost(User.GetUserId(), request.Content);
        return _mapper.Map<CreatePostResponse>(post);
    }

    [HttpGet("user/{userId}")]
    public async Task<IEnumerable<PostResponse>> GetUserPosts(Guid userId)
    {
        var posts = await _postsRepository.GetUserPosts(userId);

        return posts.Select(p => new PostResponse
        {
            Id = p.Id,
            UserId = p.UserId,
            Content = p.Content,
            Likes = p.Likes,
            CreatedAt = p.CreatedAt
        });
    }

    [HttpGet("mine")]
    [Authorize]
    public async Task<IEnumerable<PostResponse>> GetUsersPosts()
    {
        var posts = await _postsRepository.GetUserPosts(User.GetUserId());
        return posts.Select(_mapper.Map<PostResponse>);
    }

    [HttpPost("like")]
    [Authorize]
    public async Task<IActionResult> LikePost(LikeDislikeRequest request)
    {
        await _postsRepository.LikePost(request.PostId);
        return Ok();
    }

    [HttpPost("dislike")]
    [Authorize]
    public async Task<IActionResult> DislikePost(LikeDislikeRequest request)
    {
        await _postsRepository.DislikePost(request.PostId);
        return Ok();
    }
}
