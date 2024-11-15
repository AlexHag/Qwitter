using Microsoft.AspNetCore.Mvc;
using Qwitter.Core.Application.RestApiClient;
using Qwitter.Funds.Contract.Clients.Models;

namespace Qwitter.Funds.Contract.Clients;

[ApiHost(Host.Name, "client")]
public interface IClientService
{
    [HttpPost]
    Task Register(RegisterAsClientRequest request);
}
