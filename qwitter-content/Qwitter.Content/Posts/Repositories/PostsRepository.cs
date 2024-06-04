using Microsoft.EntityFrameworkCore;
using Qwitter.Content.Posts.Models;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Core.Application.Persistence;

namespace Qwitter.Content.Posts.Repositories;

public interface IPostsRepository
{
    Task<PostEntity> CreatePost(Guid userId, string content);
    Task<IEnumerable<PostEntity>> GetUserPosts(Guid userId);
    Task<IEnumerable<PostEntity>> GetLatestPosts(PaginationRequest request);
    Task LikePost(Guid postId);
    Task DislikePost(Guid postId);
}

public class PostsRepository : IPostsRepository
{
    private readonly AppDbContext _dbContext;

    public PostsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PostEntity> CreatePost(Guid userId, string content)
    {
        var user = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundApiException("User not found");

        var post = new PostEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            User = user,
            Content = content,
            Likes = 0,
            Dislikes = 0,
            CreatedAt = DateTime.UtcNow   
        };

        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
        return post;
    }

    public async Task DislikePost(Guid postId)
    {
        var post = await _dbContext.Posts.FindAsync(postId);
        if (post is not null)
        {
            post.Dislikes++;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<PostEntity>> GetLatestPosts(PaginationRequest request)
    {
        var posts = await _dbContext.Posts
            .Include(p => p.User)
            .OrderBy(p => p.CreatedAt)
            .Skip(request.Offset)
            .Take(request.Take)
            .ToListAsync();
        
        return posts;
    }

    public async Task<IEnumerable<PostEntity>> GetUserPosts(Guid userId)
    {
        return await _dbContext.Posts.Include(p => p.User).Where(p => p.UserId == userId).ToListAsync();
    }

    public async Task LikePost(Guid postId)
    {
        var post = await _dbContext.Posts.FindAsync(postId);
        if (post is not null)
        {
            post.Likes++;
            await _dbContext.SaveChangesAsync();
        }
    }
}
