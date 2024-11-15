using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Funds.Contract.Clients;
using Qwitter.Funds.Contract.Clients.Models;
using Qwitter.Funds.Service.Clients.Models;
using Qwitter.Funds.Service.Clients.Repositories;

namespace Qwitter.Funds.Service.Clients;

[ApiController]
[Route("client")]
[Authorize(AuthenticationSchemes = "mTLS")]
public class ClientService : ControllerBase, IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    [HttpPost]
    public async Task Register(RegisterAsClientRequest request)
    {
        var thumbprint = HttpContext.Connection.ClientCertificate!.Thumbprint;

        var existingClient = await  _clientRepository.TryGetByThumbprint(thumbprint);

        if (existingClient != null)
        {
            if (existingClient.CallbackUrl != request.CallbackUrl)
            {
                existingClient.CallbackUrl = request.CallbackUrl;
                await _clientRepository.Update(existingClient);
            }

            return;
        }

        var clientName = HttpContext.Connection.ClientCertificate!.GetNameInfo(X509NameType.SimpleName, false);

        var newClient = new ClientEntity
        {
            ClientId = Guid.NewGuid(),
            ClientName = clientName,
            ClientCertificateThumbprint = thumbprint,
            CallbackUrl = request.CallbackUrl,
        };

        await _clientRepository.Insert(newClient);
    }
}