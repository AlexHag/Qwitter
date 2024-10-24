using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Qwitter.Crypto.Currency.Contract;
using Qwitter.Crypto.Currency.Contract.Models;
using Qwitter.Crypto.Currency.Contract.Transfers;
using Qwitter.Crypto.Currency.Ethereum.Transfers.Models;

namespace Qwitter.Crypto.Currency.Ethereum.Transfers;

public class EthereumTransferService : ITransferService
{
    private readonly AlchemyConfiguration _alchemyConfiguration;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<EthereumTransferService> _logger;

    public EthereumTransferService(
        AlchemyConfiguration alchemyConfiguration,
        IHttpClientFactory httpClientFactory,
        ILogger<EthereumTransferService> logger)
    {
        _alchemyConfiguration = alchemyConfiguration;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<CryptoTransferModel?> GetTransactionByHash(string transactionHash)
    {
        var request = new
        {
            jsonrpc = "2.0",
            id = 1,
            method = "eth_getTransactionByHash",
            @params = new object[] { transactionHash }
        };

        var httpClient = _httpClientFactory.CreateClient(Currencies.Ethereum);
        var rpcResponse = await httpClient.PostAsJsonAsync($"v2/{_alchemyConfiguration.Token}", request);
        var content = await rpcResponse.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        var transactionJson = doc.RootElement.GetProperty("result");

        var transaction = transactionJson.Deserialize<GetTransactionByHashResponse>();

        if (transaction == null)
        {
            return null;
        }

        int? blockNumber = string.IsNullOrEmpty(transaction.BlockNumber) ? null : Convert.ToInt32(transaction.BlockNumber, 16);
        var amount = Web3.Convert.FromWei(Convert.ToInt64(transaction.Value, 16));
        var fee = Web3.Convert.FromWei(Convert.ToInt64(transaction.Gas, 16) * Convert.ToInt64(transaction.GasPrice, 16));

        var response = new CryptoTransferModel
        {
            BlockNumber = blockNumber,
            BlockHash = transaction.BlockHash,
            TransactionHash = transaction.Hash!,
            SourceAddress = transaction.From!,
            DestinationAddress = transaction.To!,
            Amount = amount,
            Fee = fee,
            Currency = Currencies.Ethereum
        };

        return response;
    }

    public async Task<TransactionHashModel> Transfer(string privateKey, string destinationAddress, decimal amount)
    {
        var url = $"{_alchemyConfiguration.BaseUrl}/v2/{_alchemyConfiguration.Token}";
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);

        try
        {
            var transactionHash = await web3.Eth.GetEtherTransferService().TransferEtherAsync(destinationAddress, amount);

            return new TransactionHashModel
            {
                TransactionHash = transactionHash
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error transferring ETH");
            throw;
        }
    }

    public async Task<TransactionHashModel> TransferFullBalance(string privateKey, string destinationAddress)
    {
        var url = $"{_alchemyConfiguration.BaseUrl}/v2/{_alchemyConfiguration.Token}";
        var account = new Account(privateKey);
        var web3 = new Web3(account, url);
        
        var transferService = web3.Eth.GetEtherTransferService();

        var fee1559 = await transferService.SuggestFeeToTransferWholeBalanceInEtherAsync();
        fee1559.MaxPriorityFeePerGas ??= Web3.Convert.ToWei(2, UnitConversion.EthUnit.Gwei);
        fee1559.MaxFeePerGas ??= Web3.Convert.ToWei(2, UnitConversion.EthUnit.Gwei);

        var amount = await transferService
            .CalculateTotalAmountToTransferWholeBalanceInEtherAsync(account.Address, fee1559.MaxFeePerGas.Value);

        try
        {
            var transactionHash = await transferService.TransferEtherAsync(destinationAddress, amount, fee1559.MaxPriorityFeePerGas.Value, fee1559.MaxFeePerGas.Value);

            return new TransactionHashModel
            {
                TransactionHash = transactionHash
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error transferring ETH");
            throw;
        }
    }
}
