using RestSharp;
using Qwitter.Domain.DTO;

namespace Qwitter.Domain.Api;

public interface IPaymentClient
{
    Task<UserWalletDTO> GetUserWallet(Guid userId);
}

public class PaymentClient : IPaymentClient
{
    private readonly RestClient _client;

    public PaymentClient(RestClient client)
    {
        _client = client;
    }

    public async Task<UserWalletDTO> GetUserWallet(Guid userId)
    {
        var restRequest = new RestRequest($"wallet/{userId}", Method.Get);
        
        var response = await _client.ExecuteAsync<UserWalletDTO>(restRequest);
        if (response.IsSuccessful) return response.Data!;
        
        throw new Exception(response.Content);
    }
}
