using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Users.Contract.User.Models;
using Qwitter.Users.Follows.Models;

namespace Qwitter.Users.Follows.Repositories;

public interface IFollowsRepository
{
    Task<IEnumerable<UserPublicProfile>> GetFollowers(Guid userId);
    Task StartFollowing(Guid followeeId, Guid followerId);
    Task StopFollowing(Guid followeeId, Guid followerId);
}

public class FollowersRepository : IFollowsRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public FollowersRepository(
        AppDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    // TODO: Optimize
    public async Task<IEnumerable<UserPublicProfile>> GetFollowers(Guid userId)
    {
        var followersId = await _dbContext.FollowingRelationships
            .Where(r => r.FollowerId == userId).Select(p => p.FolloweeId).ToListAsync();
        
        var followers = await _dbContext.Users.Where(p => followersId.Contains(p.UserId)).ToListAsync();
        return followers.Select(p => _mapper.Map<UserPublicProfile>(p));
    }

    public async Task StartFollowing(Guid followeeId, Guid followerId)
    {
        var followee = await _dbContext.Users.FindAsync(followeeId) ?? throw new NotFoundApiException($"Followee not found, {followeeId}");
        var follower = await _dbContext.Users.FindAsync(followerId) ?? throw new NotFoundApiException($"Follower not found, {followerId}");
        followee.FollowingCount++;
        follower.FollowerCount++;

        var relationship = new FollowingRelationshipEntity
        {
            Id = Guid.NewGuid(),
            FolloweeId = followeeId,
            FollowerId = followerId,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.FollowingRelationships.AddAsync(relationship);
        await _dbContext.SaveChangesAsync();
    }

    public async Task StopFollowing(Guid followeeId, Guid followerId)
    {
        var followee = await _dbContext.Users.FindAsync(followeeId) ?? throw new NotFoundApiException($"Followee not found, {followeeId}");
        var follower = await _dbContext.Users.FindAsync(followerId) ?? throw new NotFoundApiException($"Follower not found, {followerId}");

        var relationship = await _dbContext.FollowingRelationships
            .FirstOrDefaultAsync(r => r.FolloweeId == followeeId && r.FollowerId == followerId);

        if (relationship is null)
        {
            Console.WriteLine($"Warning: Relationship not found between followee: {followeeId} and follower: {followerId}");
            return;
        }

        followee.FollowingCount--;
        follower.FollowerCount--;

        _dbContext.FollowingRelationships.Remove(relationship);
        await _dbContext.SaveChangesAsync();
    }
}