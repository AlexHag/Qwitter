using System.Security.Cryptography.X509Certificates;
using Qwitter.Funds.Service.Clients.Models;
using Qwitter.Funds.Service.Clients.Repositories;

namespace Qwitter.Funds.Service.Clients.Handler;

public interface IClientHandler
{
    Task<ClientEntity> Get(X509Certificate2 certificate, string? url = null);
}

public class ClientHandler : IClientHandler
{
    private readonly IClientRepository _clientRepository;

    public ClientHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<ClientEntity> Get(X509Certificate2 certificate, string? url = null)
    {
        var thumbprint = certificate.Thumbprint;
        url ??= certificate.GetNameInfo(X509NameType.DnsName, false);

        var existingClient = await  _clientRepository.TryGetByThumbprint(thumbprint);

        if (existingClient != null)
        {
            if (!string.IsNullOrEmpty(url) && existingClient.CallbackUrl != url)
            {
                existingClient.CallbackUrl = url;
                await _clientRepository.Update(existingClient);
            }

            return existingClient;
        }

        var name = certificate.GetNameInfo(X509NameType.SimpleName, false);

        var newClient = new ClientEntity
        {
            ClientId = Guid.NewGuid(),
            ClientName = name,
            ClientCertificateThumbprint = thumbprint,
            CallbackUrl = url,
        };

        await _clientRepository.Insert(newClient);

        return newClient;
    }
}