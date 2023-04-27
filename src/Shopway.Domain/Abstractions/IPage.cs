namespace Shopway.Domain.Abstractions;

public interface IPage
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}