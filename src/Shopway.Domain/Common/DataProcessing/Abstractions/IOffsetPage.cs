namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IOffsetPage : IPage
{
    int PageNumber { get; init; }
}
