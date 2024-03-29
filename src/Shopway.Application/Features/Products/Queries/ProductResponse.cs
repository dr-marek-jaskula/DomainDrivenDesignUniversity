﻿using Shopway.Application.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Application.Features.Products.Queries;

public sealed record ProductResponse
(
    Ulid Id,
    string ProductName,
    string Revision,
    decimal Price,
    string UomCode,
    IReadOnlyCollection<ReviewResponse> Reviews
)
    : IResponse, IHasCursor;
