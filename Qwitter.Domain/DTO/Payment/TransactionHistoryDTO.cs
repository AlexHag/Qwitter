namespace Qwitter.Domain.DTO;

public class TransactionHistoryDTO
{
    public required string FromAddress { get; set; }
    public required string ToAddress { get; set; }
    public decimal Amount { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}