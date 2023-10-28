﻿using Shopway.Domain.EntityIds;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.RemoveOrderLine;

public sealed record RemoveOrderLineCommand
(
    OrderHeaderId OrderHeaderId,
    OrderLineId OrderLineId
) : ICommand<RemoveOrderLineResponse>;