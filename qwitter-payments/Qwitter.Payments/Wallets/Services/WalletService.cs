using Nethereum.Signer;
using Qwitter.Payments.Wallets.Models;
using Qwitter.Payments.Wallets.Repositories;

namespace Qwitter.Payments.Wallets.Services;

public interface IWalletService
{
    Task<WalletModel> CreateWallet(Guid transactionId);
}

public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;

    public WalletService(IWalletRepository walletRepository)
    {
        _walletRepository = walletRepository;
    }

    public async Task<WalletModel> CreateWallet(Guid transactionId)
    {
        var key = EthECKey.GenerateKey();

        var wallet = new WalletEntity
        {
            Id = Guid.NewGuid(),
            Address = key.GetPublicAddress(),
            PrivateKey = key.GetPrivateKey()
        };

        await _walletRepository.InsertWallet(wallet);

        return new WalletModel
        {
            Id = wallet.Id,
            Address = wallet.Address
        };
    }
}
