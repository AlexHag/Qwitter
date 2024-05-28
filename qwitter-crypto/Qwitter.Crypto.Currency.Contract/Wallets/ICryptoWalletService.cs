using Qwitter.Crypto.Currency.Contract.Wallets.Models;

namespace Qwitter.Crypto.Currency.Contract.Wallets;

public interface ICryptoWalletService
{
    Task<WalletModel> CreateWallet();
    Task<IEnumerable<CryptoTransfer>> GetWalletTransfers(string address);
    Task<IEnumerable<CryptoTransfer>> GetWalletTransferSinceBlockNumber(string address, int blockNumber);
    Task<IEnumerable<CryptoTransfer>> GetWalletTransferSinceBlockHash(string address, string blockHash);
}