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
}
