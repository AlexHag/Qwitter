using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Qwitter.Funds.Contract.Clients;
using Qwitter.Funds.Contract.Clients.Models;
using Qwitter.Funds.Service.Clients.Handler;

namespace Qwitter.Funds.Service.Clients;

[ApiController]
[Route("client")]
[Authorize(AuthenticationSchemes = "mTLS")]
public class ClientService : ControllerBase, IClientService
{
    private readonly IClientHandler _clientHandler;

    public ClientService(IClientHandler clientHandler)
    {
        _clientHandler = clientHandler;
    }

    [HttpPost]
    public async Task Register(RegisterAsClientRequest request)
    {
        await _clientHandler.Get(HttpContext.Connection.ClientCertificate!, request.CallbackUrl);
    }
}