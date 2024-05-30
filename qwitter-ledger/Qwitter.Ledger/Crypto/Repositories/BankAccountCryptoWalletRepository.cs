
using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.Crypto.Models;

namespace Qwitter.Ledger.Crypto.Repositories;

public interface IBankAccountCryptoWalletRepository
{
    Task Insert(BankAccountCryptoWalletEntity entity);
    Task<BankAccountCryptoWalletEntity?> GetByWalletId(Guid walletId);
    Task<BankAccountCryptoWalletEntity?> GetByBankAccountIdAndCurrency(Guid bankAccountId, string currency);
}

public class BankAccountCryptoWalletRepository : IBankAccountCryptoWalletRepository
{
    private readonly AppDbContext _dbContext;

    public BankAccountCryptoWalletRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(BankAccountCryptoWalletEntity entity)
    {
        await _dbContext.BankAccountCryptoWallets.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BankAccountCryptoWalletEntity?> GetByWalletId(Guid walletId)
    {
        return await _dbContext.BankAccountCryptoWallets.FirstOrDefaultAsync(p => p.WalletId == walletId);
    }

    public async Task<BankAccountCryptoWalletEntity?> GetByBankAccountIdAndCurrency(Guid bankAccountId, string currency)
    {
        return await _dbContext.BankAccountCryptoWallets.FirstOrDefaultAsync(p => p.BankAccountId == bankAccountId && p.Currency == currency);
    }
}