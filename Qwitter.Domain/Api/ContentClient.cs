using RestSharp;
using Qwitter.Domain.DTO;

namespace Qwitter.Domain.Api;

public interface IContentClient
{
    Task<PostDTO> CreatePost(CreatePostDTO request);
    Task<PostDTO> GetPostById(Guid postId);
    Task<List<PostDTO>> GetUserPosts(string username);
    Task<CommentDTO> CreateComment(CreateCommentDTO request);
}

public class ContentClient : IContentClient
{
    private readonly IRestClient _client;
    public ContentClient(IRestClient client)
    {
        _client = client;
    }

    public async Task<PostDTO> CreatePost(CreatePostDTO request)
    {
        var restRequest = new RestRequest($"posts/create", Method.Post)
            .AddBody(request);
        
        var response = await _client.ExecuteAsync<PostDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;
        
        throw new Exception(response.Content);
    }

    public async Task<PostDTO> GetPostById(Guid postId)
    {
        var restRequest = new RestRequest($"posts/{postId}", Method.Get);
        
        var response = await _client.ExecuteAsync<PostDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;
        
        throw new Exception(response.Content);
    }

    public async Task<List<PostDTO>> GetUserPosts(string username)
    {
        var restRequest = new RestRequest($"posts/user/{username}", Method.Get);
        
        var response = await _client.ExecuteAsync<List<PostDTO>>(restRequest);
        if (response.IsSuccessful) return response.Data!;
        
        throw new Exception(response.Content);
    }

    public async Task<CommentDTO> CreateComment(CreateCommentDTO request)
    {
        var restRequest = new RestRequest($"comments/{request.PostId}", Method.Post)
            .AddBody(request);
        
        var response = await _client.ExecuteAsync<CommentDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;
        
        throw new Exception(response.Content);
    }
}
