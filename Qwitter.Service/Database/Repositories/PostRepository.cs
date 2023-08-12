using Qwitter.Models.Entities;
using Qwitter.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace Qwitter.Service.Database.Repositories;

public interface IPostRepository
{
    Task<PostResponse> CreatePost(Guid userId, string content);
    Task<PostResponse?> GetPostById(Guid postId);
    Task<List<PostResponse>> GetUsersPosts(Guid userId);
}

public class PostRepository : IPostRepository
{
    private readonly DatabaseContext _dbContext;

    public PostRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PostResponse> CreatePost(Guid userId, string content)
    {
        var user = await _dbContext.Users.FindAsync(userId) ?? throw new Exception($"Could not find user with id: {userId}");
        
        var post = new Post
        {
            PostId = Guid.NewGuid(),
            Author = user,
            Content = content,
            CreatedAt = DateTime.Now,
            Likes = 0,
            Dislikes = 0,
            IsPremium = user.IsPremium
        };

        user.Posts.Add(post);
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        var postResponse = new PostResponse
        {
            PostId = post.PostId,
            AuthorUserId = user.UserId,
            AuthorUsername = user.Username,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Likes = post.Likes,
            Dislikes = post.Dislikes,
            IsPremium = post.IsPremium
        };

        return postResponse;
    }

    public async Task<PostResponse?> GetPostById(Guid postId)
    {
        var post = await _dbContext.Posts.Include(p => p.Author).FirstOrDefaultAsync(p => p.PostId == postId);
        if (post is null) return null;

        var postResponse = new PostResponse
        {
            PostId = post.PostId,
            AuthorUserId = post.Author.UserId,
            AuthorUsername = post.Author.Username,
            Content = post.Content,
            CreatedAt = post.CreatedAt,
            Likes = post.Likes,
            Dislikes = post.Dislikes,
            IsPremium = post.IsPremium
        };
        
        return postResponse;
    }

    public async Task<List<PostResponse>> GetUsersPosts(Guid userId)
    {
        var user = await _dbContext.Users.Include(p => p.Posts).FirstOrDefaultAsync(p => p.UserId == userId);
        if (user is null) throw new Exception($"User with id: {userId} not found");

        var postsResponse = new List<PostResponse>();
        postsResponse.AddRange(user.Posts.Select(p => new PostResponse
        {
            PostId = p.PostId,
            AuthorUserId = p.Author.UserId,
            AuthorUsername = p.Author.Username,
            Content = p.Content,
            CreatedAt = p.CreatedAt,
            Likes = p.Likes,
            Dislikes = p.Dislikes,
            IsPremium = p.IsPremium
        }));
        
        return postsResponse;
    }
}