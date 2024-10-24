using Qwitter.Crypto.Currency.Contract.Models;
using Qwitter.Crypto.Currency.Contract.Wallets.Models;

namespace Qwitter.Crypto.Currency.Contract.Wallets;

public interface ICryptoWalletService
{
    Task<WalletModel> CreateWallet();
    Task<IEnumerable<CryptoTransferModel>> GetWalletTransfers(string address);
    Task<IEnumerable<CryptoTransferModel>> GetWalletTransferSinceBlockNumber(string address, int blockNumber);
    Task<IEnumerable<CryptoTransferModel>> GetWalletTransferSinceBlockHash(string address, string blockHash);
}