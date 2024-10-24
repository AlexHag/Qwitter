using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Nethereum.Signer;
using Nethereum.Util;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Qwitter.Crypto.Currency.Contract;
using Qwitter.Crypto.Currency.Contract.Models;
using Qwitter.Crypto.Currency.Contract.Wallets;
using Qwitter.Crypto.Currency.Contract.Wallets.Models;
using Qwitter.Crypto.Currency.Ethereum.Wallet.Models;

namespace Qwitter.Crypto.Currency.Ethereum.Wallet;

public class EthereumWalletService : ICryptoWalletService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AlchemyConfiguration _alchemyConfiguration;
    private readonly ILogger<EthereumWalletService> _logger;

    public EthereumWalletService(
        IHttpClientFactory httpClientFactory,
        AlchemyConfiguration alchemyConfiguration,
        ILogger<EthereumWalletService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _alchemyConfiguration = alchemyConfiguration;
        _logger = logger;
    }

    public Task<WalletModel> CreateWallet()
    {
        var key = EthECKey.GenerateKey();

        var wallet = new WalletModel
        {
            Currency = Currencies.Ethereum,
            Address = key.GetPublicAddress(),
            PrivateKey = key.GetPrivateKey()
        };

        return Task.FromResult(wallet);
    }

    public async Task<IEnumerable<CryptoTransferModel>> GetWalletTransfers(string address)
    {
        return await GetWalletTransferSinceBlockNumber(address, 0);
    }

    public Task<IEnumerable<CryptoTransferModel>> GetWalletTransferSinceBlockHash(string address, string blockHash)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CryptoTransferModel>> GetWalletTransferSinceBlockNumber(string address, int blockNumber)
    {
        var blockNumberHex = string.Format("0x{0:X}", blockNumber);
        var request = new 
        {
            jsonrpc = "2.0",
            id = 1,
            method = "alchemy_getAssetTransfers",
            @params = new[] 
            { 
                new 
                { 
                    fromBlock = blockNumberHex,
                    toBlock = "latest",
                    toAddress = address,
                    category = new[] 
                    { 
                        "external"
                    }
                } 
            } 
        };

        var httpClient = _httpClientFactory.CreateClient(Currencies.Ethereum);
        var response = await httpClient.PostAsJsonAsync($"v2/{_alchemyConfiguration.Token}", request);
        var content = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(content);
        var transfersJson = doc.RootElement.GetProperty("result").GetProperty("transfers");

        var transfers = transfersJson.Deserialize<IEnumerable<AssetTransfer>>();

        if (transfers is null)
        {
            return [];
        }

        return transfers.Select(p => new CryptoTransferModel
        {
            BlockNumber = Convert.ToInt32(p.BlockNum, 16),
            TransactionHash = p.Hash!,
            SourceAddress = p.From!,
            DestinationAddress = p.To!,
            Amount = p.Value,
            Currency = Currencies.Ethereum
        });
    }
}
