using Nethereum.Signer;
using Qwitter.Payments.Wallets.Models;
using Qwitter.Payments.Wallets.Repositories;

namespace Qwitter.Payments.Wallets.Services;

public interface IWalletService
{
    Task<WalletModel> CreateWallet(Guid transactionId, string currency);
}

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;

    public WalletService(
        IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<WalletModel> CreateWallet(Guid transactionId, string currency)
    {
        if (currency != "ETH")
        {
            throw new NotSupportedException("Currency not supported");
        }

        var key = EthECKey.GenerateKey();

        var wallet = new WalletEntity
        {
            Id = Guid.NewGuid(),
            Currency = currency,
            Address = key.GetPublicAddress(),
            PrivateKey = key.GetPrivateKey()
        };

        await _walletRepository.InsertWallet(wallet);

        return new WalletModel
        {
            Id = wallet.Id,
            Currency = currency,
            Address = wallet.Address
        };
    }
}
