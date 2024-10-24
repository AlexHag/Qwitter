using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Crypto.Service.Wallet.Models;

namespace Qwitter.Crypto.Service.Wallet.Repositories;

public interface IWalletRepository
{
    Task InsertWallet(WalletEntity wallet);
    Task Update(WalletEntity wallet);
    Task<WalletEntity> GetById(Guid walletId);
    Task<WalletEntity?> GetByAddress(string address);
}

public class WalletRepository : IWalletRepository
{
    private readonly ServiceDbContext _dbContext;

    public WalletRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<WalletEntity> GetById(Guid walletId)
        => await _dbContext.Wallets.FindAsync(walletId) ?? throw new NotFoundApiException("Wallet not found");

    public async Task InsertWallet(WalletEntity wallet)
    {
        await _dbContext.Wallets.AddAsync(wallet);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<WalletEntity?> GetByAddress(string address)
        => await _dbContext.Wallets.FirstOrDefaultAsync(w => w.Address == address);

    public Task Update(WalletEntity wallet)
    {
        _dbContext.Wallets.Update(wallet);
        return _dbContext.SaveChangesAsync();
    }
}