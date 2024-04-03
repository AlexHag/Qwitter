using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
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
    private readonly HttpClient _httpClient;
    private readonly string _url;

    public PaymentProviderService(string url)
    {
        _url = url;
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(url)
        };
    }

    public async Task<decimal> GetAmountReceived(string address)
    {
        var request = new { jsonrpc = "2.0", id = 1, method = "alchemy_getAssetTransfers", @params = new[] { new { fromBlock = "0x0", toBlock = "latest", toAddress = address, category = new[] { "external" } } } };
        var response = await _httpClient.PostAsJsonAsync("", request);
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
        var web3 = new Web3(account, _url);
        
        var transferService = web3.Eth.GetEtherTransferService();

        var fee1559 = await transferService.SuggestFeeToTransferWholeBalanceInEtherAsync();
        fee1559.MaxPriorityFeePerGas ??= Web3.Convert.ToWei(2, UnitConversion.EthUnit.Gwei);
        fee1559.MaxFeePerGas ??= Web3.Convert.ToWei(2, UnitConversion.EthUnit.Gwei);

        var amount = await transferService
            .CalculateTotalAmountToTransferWholeBalanceInEtherAsync(account.Address, fee1559.MaxFeePerGas.Value);

        Console.WriteLine(amount);

        try
        {
            await transferService.TransferEtherAndWaitForReceiptAsync(address, amount, fee1559.MaxPriorityFeePerGas.Value, fee1559.MaxFeePerGas.Value);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }

    public Task<bool> Transfer(string key, string address, decimal amount)
    {
        throw new NotImplementedException();
    }
}