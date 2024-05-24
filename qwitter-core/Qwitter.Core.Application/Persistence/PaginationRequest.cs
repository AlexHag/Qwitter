namespace Qwitter.Core.Application.Persistence;

public class PaginationRequest
{
    public int Take { get; set; }
    public int Offset { get; set; }
}