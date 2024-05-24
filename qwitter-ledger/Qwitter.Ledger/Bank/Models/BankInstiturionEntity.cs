namespace Qwitter.Ledger.Bank.Models;

public class BankInstitutionEntity
{
    public Guid Id { get; set; }
    public required string RoutingNumber { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
