using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Orders.Commands.StartPaymentProcess;

public sealed record StartPaymentProcessResponse(string SessionId, string ClientSecret) : IResponse;
