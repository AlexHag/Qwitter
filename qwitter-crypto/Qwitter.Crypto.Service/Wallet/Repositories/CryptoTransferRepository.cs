using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Qwitter.Crypto.Service.Wallet.Models;

namespace Qwitter.Crypto.Service.Wallet.Repositories;

public interface ICryptoTransferRepository
{
    Task Insert(CryptoTransferEntity transfer);
    Task<IEnumerable<CryptoTransferEntity>> GetByDestinationAddress(string address);
}

public class CryptoTransferRepository : ICryptoTransferRepository
{
    private readonly ServiceDbContext _dbContext;

    public CryptoTransferRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Insert(CryptoTransferEntity transfer)
    {
        await _dbContext.CryptoTransfers.AddAsync(transfer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<CryptoTransferEntity>> GetByDestinationAddress(string address)
    {
        return await _dbContext.CryptoTransfers
            .Where(t => t.DestinationAddress == address)
            .ToListAsync();
    }
}