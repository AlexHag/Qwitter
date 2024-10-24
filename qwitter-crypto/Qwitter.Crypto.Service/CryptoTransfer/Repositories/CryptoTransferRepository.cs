using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Crypto.Service.CryptoTransfer.Models;

namespace Qwitter.Crypto.Service.CryptoTransfer.Repositories;

public interface ICryptoTransferRepository
{
    Task Insert(CryptoTransferEntity transfer);
    Task<CryptoTransferEntity> GetById(Guid id);
    Task Update(CryptoTransferEntity transfer);
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

    public async Task Update(CryptoTransferEntity transfer)
    {
        _dbContext.CryptoTransfers.Update(transfer);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<CryptoTransferEntity>> GetByDestinationAddress(string address)
    {
        return await _dbContext.CryptoTransfers
            .Where(t => t.DestinationAddress == address)
            .ToListAsync();
    }

    public async Task<CryptoTransferEntity> GetById(Guid id)
        => await _dbContext.CryptoTransfers.FirstOrDefaultAsync(t => t.TransactionId == id) ?? throw new NotFoundApiException("Transfer not found");
}