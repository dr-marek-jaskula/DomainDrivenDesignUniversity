namespace Shopway.Application.Abstractions.CQRS;

public interface IPageQuery<out TResponse, TFilter, TSortBy> : IQuery<TResponse>
    where TResponse : IResponse
    where TFilter : IFilter
    where TSortBy : ISortBy
{
    int PageNumber { get; }
    int PageSize { get; }
    TFilter Filter { get; }
    TSortBy Order { get; }
}