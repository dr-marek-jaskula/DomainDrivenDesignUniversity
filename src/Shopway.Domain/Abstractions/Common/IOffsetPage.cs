namespace Shopway.Domain.Abstractions.Common;

public interface IOffsetPage : IPage
{
    int PageNumber { get; init; }
}