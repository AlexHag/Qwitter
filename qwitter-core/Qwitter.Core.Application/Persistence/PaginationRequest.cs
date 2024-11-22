namespace Qwitter.Core.Application.Persistence;

public class PaginationRequest<T>
{
    public int Take { get; set; }
    public int Skip { get; set; }
    public T? Filter { get; set; }
}