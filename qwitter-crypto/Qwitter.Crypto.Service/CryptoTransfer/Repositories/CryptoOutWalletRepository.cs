using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Crypto.Service.CryptoTransfer.Models;

namespace Qwitter.Crypto.Service.CryptoTransfer.Repositories;

public interface ICryptoOutWalletRepository
{
    Task<CryptoOutWalletEntity> GetWalletByCrurrency(string currency);
}

public class CryptoOutWalletRepository : ICryptoOutWalletRepository
{
    private readonly ServiceDbContext _dbContext;

    public CryptoOutWalletRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CryptoOutWalletEntity> GetWalletByCrurrency(string currency)
        => await _dbContext.CryptoOutWallets.FirstOrDefaultAsync(x => x.Currency == currency) ?? throw new NotFoundApiException("Wallet not found");
}