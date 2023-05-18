using Shopway.Domain.Abstractions;
using Shopway.Domain.Errors;

namespace Shopway.Application.CQRS;

/// <summary>
/// Contains information about corresponding batch request
/// </summary>
/// <param name="Key">Unique key that distinguish request and respective entity. Usually it is a composed key of some entity fields</param>
/// <param name="Status">Error if at least one error occurs. Otherwise, one of success statuses</param>
/// <param name="Errors">The list of errors for corresponding batch request</param>
public sealed record BatchResponseEntry
(
    IUniqueKey Key,
    BatchEntryStatus Status,
    IList<Error> Errors
);