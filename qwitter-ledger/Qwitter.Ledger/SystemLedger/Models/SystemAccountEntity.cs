
namespace Qwitter.SystemLedger.Models;

public class SystemAccountEntity
{
    public int Id { get; set; }
    public required string Currency { get; set; }
    public decimal Balance { get; set; }
}