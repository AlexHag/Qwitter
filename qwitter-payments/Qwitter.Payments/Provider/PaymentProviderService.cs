using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Qwitter.Payments.Provider.Configuration;
using System.Text.Json;

namespace Qwitter.Payments.Provider;

public interface IPaymentProviderService
{
    Task<decimal> GetAmountReceived(string address);
    Task<bool> Transfer(string key, string address);
    Task<bool> Transfer(string key, string address, decimal amount);
}

public class PaymentProviderService : IPaymentProviderService
{
    private readonly ILogger<PaymentProviderService> _logger;
    private readonly HttpClient _httpClient;
    private readonly PaymentProviderCredentials _credentials;

    public PaymentProviderService(
        ILogger<PaymentProviderService> logger,
        HttpClient httpClient,
        PaymentProviderCredentials credentials)
    {
        _logger = logger;
        _httpClient = httpClient;
        _credentials = credentials;
    }

    public async Task<decimal> GetAmountReceived(string address)
    {
        var request = new { jsonrpc = "2.0", id = 1, method = "alchemy_getAssetTransfers", @params = new[] { new { fromBlock = "0x0", toBlock = "latest", toAddress = address, category = new[] { "external" } } } };
        var response = await _httpClient.PostAsJsonAsync($"v2/{_credentials.Token}", request);
        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        var root = doc.RootElement;
        var transfersArray = root.GetProperty("result").GetProperty("transfers");

        var totalValue = 0m;

        foreach (JsonElement transfer in transfersArray.EnumerateArray())
        {
            totalValue += transfer.GetProperty("value").GetDecimal();
        }

        return totalValue;
    }

    public async Task<bool> Transfer(string key, string address)
    {
        var account = new Account(key);
        var web3 = new Web3(account, _credentials.Token);
        
        var transferService = web3.Eth.GetEtherTransferService();

        var fee1559 = await transferService.SuggestFeeToTransferWholeBalanceInEtherAsync();
        fee1559.MaxPriorityFeePerGas ??= Web3.Convert.ToWei(2, UnitConversion.EthUnit.Gwei);
        fee1559.MaxFeePerGas ??= Web3.Convert.ToWei(2, UnitConversion.EthUnit.Gwei);

        var amount = await transferService
            .CalculateTotalAmountToTransferWholeBalanceInEtherAsync(account.Address, fee1559.MaxFeePerGas.Value);

        try
        {
            await transferService.TransferEtherAndWaitForReceiptAsync(address, amount, fee1559.MaxPriorityFeePerGas.Value, fee1559.MaxFeePerGas.Value);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error transferring funds");
            return false;
        }
    }

    public Task<bool> Transfer(string key, string address, decimal amount)
    {
        throw new NotImplementedException();
    }
}