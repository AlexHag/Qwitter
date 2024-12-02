using Microsoft.EntityFrameworkCore;
using Qwitter.Core.Application.Exceptions;
using Qwitter.Funds.Service.Clients.Models;

namespace Qwitter.Funds.Service.Clients.Repositories;

public interface IClientRepository
{
    Task Insert(ClientEntity client);
    Task Update(ClientEntity client);
    Task<ClientEntity> GetById(Guid clientId);
    Task<ClientEntity?> TryGetByThumbprint(string thumbprint);
    Task<ClientEntity> GetByThumbprint(string thumbprint);
}

public class ClientRepository : IClientRepository
{
    private readonly ServiceDbContext _dbContext;

    public ClientRepository(ServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ClientEntity> GetByThumbprint(string thumbprint)
    {
        var entity = await _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientCertificateThumbprint == thumbprint);

        if (entity == null)
        {
            throw new NotFoundApiException($"Client with thumbprint {thumbprint} not found");
        }

        return entity;
    }

    public async Task Insert(ClientEntity client)
    {
        client.CreatedAt = DateTime.UtcNow;
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<ClientEntity> GetById(Guid clientId) 
    {
        var entity = await _dbContext.Clients.FindAsync(clientId);

        if (entity == null)
        {
            throw new NotFoundApiException($"Client {clientId} not found");
        }

        return entity;
    }

    public Task<ClientEntity?> TryGetByThumbprint(string thumbprint)
        => _dbContext.Clients.FirstOrDefaultAsync(c => c.ClientCertificateThumbprint == thumbprint);

    public async Task Update(ClientEntity client)
    {
        client.UpdatedAt = DateTime.UtcNow;
        _dbContext.Clients.Update(client);
        await _dbContext.SaveChangesAsync();
    }
}