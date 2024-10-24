using Qwitter.Crypto.Currency.Contract.Models;

namespace Qwitter.Crypto.Currency.Contract.Transfers;

public interface ITransferService
{
    Task<CryptoTransferModel?> GetTransactionByHash(string transactionHash);
    Task<TransactionHashModel> Transfer(string privateKey, string destinationAddress, decimal amount);
    Task<TransactionHashModel> TransferFullBalance(string privateKey, string destinationAddress);
}