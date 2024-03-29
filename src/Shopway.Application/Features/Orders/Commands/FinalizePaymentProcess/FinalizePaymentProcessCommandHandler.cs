using Microsoft.Extensions.Configuration;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.ValueObjects;

namespace Shopway.Application.Features.Orders.Commands.FinalizePaymentProcess;

internal sealed class FinalizePaymentProcessCommandHandler
(
    IOrderHeaderRepository orderHeaderRepository, 
    IValidator validator, 
    IConfiguration configuration
)
    : ICommandHandler<FinalizePaymentProcessCommand>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;
    private readonly IValidator _validator = validator;
    private readonly IConfiguration _configuration = configuration;

    public async Task<IResult> Handle(FinalizePaymentProcessCommand command, CancellationToken cancellationToken)
    {
        var secretHash = GetHashOfSecretSharedWithPaymentGateway();
        var sessionIdResult = SessionId.Create(command.SessionId);

        _validator
            .If(secretHash != command.SecretHash, Error.VerificationError("Provided secret is invalid"))
            .Validate(sessionIdResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        OrderHeaderId? orderHeaderId = await _orderHeaderRepository.GetOrderHeaderIdByPaymentSessionId(sessionIdResult.Value, cancellationToken);

        _validator
            .If(orderHeaderId is null, Error.NotFound<OrderHeader>(sessionIdResult.Value.Value));

        if (_validator.IsInvalid)
        {
            return _validator.Failure();
        }

        var orderHeader = await _orderHeaderRepository.GetByIdWithIncludesAsync((OrderHeaderId)orderHeaderId!, cancellationToken, oh => oh.Payment);
        orderHeader.Payment.ProcessPayment(command.WasPaymentSuccessful);

        return Result.Success();
    }

    /// <summary>
    /// DO NOT STORE SECRET IN UNSECURE STORAGE LIKE appsettings.json. This was done just to simplicity purpose.
    /// </summary>
    private string GetHashOfSecretSharedWithPaymentGateway()
    {
        var secret = _configuration
            .GetValue<string>("PaymentGatewaySecret")!;

        return HashUtilities.ComputeSha256Hash(secret);
    }
}