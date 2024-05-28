using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Qwitter.Crypto.Wallets.Models;

namespace Qwitter.Crypto.Wallets.Repositories;

public interface ICryptoTransferRepository
{
    Task Insert(CryptoTransferEntity transfer);
    Task<IEnumerable<CryptoTransferEntity>> GetByToAddress(string address);
}

public class CryptoTransferRepository : ICryptoTransferRepository
{
    private readonly AppDbContext _dbContext;

    public CryptoTransferRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(CryptoTransferEntity transfer)
    {
        await _dbContext.CryptoTransfers.AddAsync(transfer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<CryptoTransferEntity>> GetByToAddress(string address)
    {
        return await _dbContext.CryptoTransfers
            .Where(t => t.To == address)
            .ToListAsync();
    }
}