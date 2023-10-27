using Qwitter.Domain.DTO;

using RestSharp;

namespace Qwitter.Domain.Api;

public interface IUserClient
{
    Task<UserDTO> Login(UsernamePasswordDTO request);
    Task<UserDTO> Register(UsernamePasswordDTO request);
    Task<UserDTO> UpdateBio(UpdateBioDTO request);
    Task<UserDTO> GetUser(Guid userId);
    Task UpdateUsername(UpdateUsernameDTO request);
}

public class UserClient : IUserClient
{
    private readonly RestClient _client;

    public UserClient(RestClient client)
    {
        _client = client;
    }

    public async Task<UserDTO> GetUser(Guid userId)
    {
        var restRequest = new RestRequest($"user/{userId}", Method.Get);
        
        var response = await _client.ExecuteAsync<UserDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;
        
        throw new Exception(response.Content);
    }

    public async Task<UserDTO> Login(UsernamePasswordDTO request)
    {
        var restRequest = new RestRequest("user/login", Method.Post)
            .AddBody(request);
        
        var response = await _client.ExecuteAsync<UserDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;

        throw new Exception(response.Content);
    }

    public async Task<UserDTO> Register(UsernamePasswordDTO request)
    {
        var restRequest = new RestRequest("user/register", Method.Post)
            .AddBody(request);
        
        var response = await _client.ExecuteAsync<UserDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;

        throw new Exception(response.Content);
    }

    public async Task<UserDTO> UpdateBio(UpdateBioDTO request)
    {
        var restRequest = new RestRequest("user/bio", Method.Patch)
            .AddBody(request);
        
        var response = await _client.ExecuteAsync<UserDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;
        
        throw new Exception(response.Content);
    }

    public async Task UpdateUsername(UpdateUsernameDTO request)
    {
        var restRequest = new RestRequest("user/username", Method.Patch)
            .AddBody(request);
        
        var response = await _client.ExecuteAsync(restRequest);

        if (response.IsSuccessful)
            throw new Exception(response.Content);
    }
}