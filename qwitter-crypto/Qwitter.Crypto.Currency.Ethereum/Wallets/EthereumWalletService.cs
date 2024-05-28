using System.Net.Http.Json;
using System.Text.Json;
using Nethereum.Signer;
using Qwitter.Crypto.Currency.Contract;
using Qwitter.Crypto.Currency.Contract.Wallets;
using Qwitter.Crypto.Currency.Contract.Wallets.Models;

namespace Qwitter.Crypto.Currency.Ethereum;

public class EthereumWalletService : ICryptoWalletService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AlchemyConfiguration _alchemyConfiguration;

    public EthereumWalletService(
        IHttpClientFactory httpClientFactory,
        AlchemyConfiguration alchemyConfiguration)
    {
        _httpClientFactory = httpClientFactory;
        _alchemyConfiguration = alchemyConfiguration;
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

    public async Task<IEnumerable<CryptoTransfer>> GetWalletTransfers(string address)
    {
        return await GetWalletTransferSinceBlockNumber(address, 0);
    }

    public Task<IEnumerable<CryptoTransfer>> GetWalletTransferSinceBlockHash(string address, string blockHash)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<CryptoTransfer>> GetWalletTransferSinceBlockNumber(string address, int blockNumber)
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

        return transfers.Select(p => new CryptoTransfer
        {
            BlockNumber = Convert.ToInt32(p.BlockNum, 16),
            TransactionHash = p.Hash!,
            From = p.From!,
            To = p.To!,
            Amount = p.Value,
            Currency = Currencies.Ethereum
        });
    }
}