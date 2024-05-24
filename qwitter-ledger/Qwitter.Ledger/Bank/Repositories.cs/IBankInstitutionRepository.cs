using Microsoft.EntityFrameworkCore;
using Qwitter.Ledger.Bank.Models;

namespace Qwitter.Ledger.Bank.Repositories;

public interface IBankInstitutionRepository
{
    Task<BankInstitutionEntity?> GetBankInstitutionById(Guid id);
    Task<BankInstitutionEntity?> GetBankInstitutionByRoutingNumber(string routingNumber);
    Task<BankInstitutionEntity?> GetBankInstitutionByName(string name);
    Task<BankInstitutionEntity> InsertBankInstitution(BankInstitutionEntity bankInstitution);
    Task<BankInstitutionEntity> UpdateBankInstitution(BankInstitutionEntity bankInstitution);
}

public class BankInstitutionRepository : IBankInstitutionRepository
{
    private readonly AppDbContext _dbContext;

    public BankInstitutionRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BankInstitutionEntity?> GetBankInstitutionById(Guid id)
    {
        return await _dbContext.BankInstitutions.FindAsync(id);
    }

    public async Task<BankInstitutionEntity?> GetBankInstitutionByRoutingNumber(string routingNumber)
    {
        return await _dbContext.BankInstitutions.FirstOrDefaultAsync(b => b.RoutingNumber == routingNumber);
    }

    public async Task<BankInstitutionEntity?> GetBankInstitutionByName(string name)
    {
        return await _dbContext.BankInstitutions.FirstOrDefaultAsync(b => b.Name == name);
    }

    public async Task<BankInstitutionEntity> InsertBankInstitution(BankInstitutionEntity bankInstitution)
    {
        await _dbContext.BankInstitutions.AddAsync(bankInstitution);
        await _dbContext.SaveChangesAsync();
        return bankInstitution;
    }

    public async Task<BankInstitutionEntity> UpdateBankInstitution(BankInstitutionEntity bankInstitution)
    {
        _dbContext.BankInstitutions.Update(bankInstitution);
        await _dbContext.SaveChangesAsync();
        return bankInstitution;
    }
}