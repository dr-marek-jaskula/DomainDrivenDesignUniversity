using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Application.Features.Orders.Queries;

public sealed record PaymentResponse
(
    Ulid Id,
    string Status,
    string IsRefunded
)
    : IResponse, IHasCursor;