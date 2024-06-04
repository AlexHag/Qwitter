using Qwitter.Content.Comments.Models;
using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Content.Comments.Repositories;

public interface ICommentRepository
{
    Task<CommentEntity> InsertComment(Guid userId, Guid postId, string content);
}

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _dbContext;

    public CommentRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CommentEntity> InsertComment(Guid userId, Guid postId, string content)
    {
        var user = await _dbContext.Users.FindAsync(userId) ?? throw new NotFoundApiException("User not found");
        var post = await _dbContext.Posts.FindAsync(postId) ?? throw new NotFoundApiException("Post not found");

        var comment = new CommentEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            PostId = postId,
            Post = post,
            User = user,
            Content = content,
            Likes = 0,
            Dislikes = 0,
            CreatedAt = DateTime.UtcNow
        };

        post.Comments.Add(comment);
        await _dbContext.SaveChangesAsync();

        return comment;
    }
}