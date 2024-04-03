
using Qwitter.Payments.Wallets.Services;

namespace Qwitter.Payments.Wallets.Repositories;

public interface IWalletRepository
{
    Task InsertWallet(WalletEntity wallet);
    Task<WalletEntity?> GetWalletById(Guid walletId);
}

public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _dbContext;

    public WalletRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WalletEntity?> GetWalletById(Guid walletId)
    {
        return await _dbContext.Wallets.FindAsync(walletId);
    }

    public async Task InsertWallet(WalletEntity wallet)
    {
        await _dbContext.Wallets.AddAsync(wallet);
        await _dbContext.SaveChangesAsync();
    }
}