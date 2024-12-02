using Qwitter.Core.Application.Exceptions;

namespace Qwitter.Funds.Service.Clients.Models;

public class ClientEntity
{
    public Guid ClientId { get; set; }
    public required string ClientName { get; set; }
    public required string ClientCertificateThumbprint { get; set; }
    public bool CanAllocateFundsIn { get; set; }
    public bool CanSettleFundsOut { get; set; }
    public required string CallbackUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public void Authorize(Guid clientId)
    {
        if (ClientId != clientId)
        {
            throw new ForbiddenApiException($"Client {ClientId} is not authorized to perform this action");
        }
    }
}
