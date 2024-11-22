namespace Qwitter.Core.Application.Persistence;

public class PaginationResponse<T>
{
    public int Count { get; set; }
    public List<T> Items { get; set; } = [];
}