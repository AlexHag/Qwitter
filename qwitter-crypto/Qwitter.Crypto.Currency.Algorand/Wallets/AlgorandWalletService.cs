using Algorand;
using Qwitter.Crypto.Currency.Contract;
using Qwitter.Crypto.Currency.Contract.Wallets;
using Qwitter.Crypto.Currency.Contract.Wallets.Models;

namespace Qwitter.Crypto.Currency.Algorand.Wallets;

public class AlgorandWalletService : ICryptoWalletService
{
    public Task<WalletModel> CreateWallet()
    {
        var account = new Account();

        var keyBytes = account.GetClearTextPrivateKey();
        var keyHex = BitConverter.ToString(keyBytes).Replace("-", string.Empty);

        var wallet = new WalletModel
        {
            Currency = Currencies.Algorand,
            Address = account.Address.ToString(),
            PrivateKey = keyHex
        };

        return Task.FromResult(wallet);
    }

    public Task<IEnumerable<CryptoTransfer>> GetWalletTransfers(string address)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CryptoTransfer>> GetWalletTransferSinceBlockHash(string address, string blockHash)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CryptoTransfer>> GetWalletTransferSinceBlockNumber(string address, int blockNumber)
    {
        throw new NotImplementedException();
    }
}