using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Shopway.Application.Abstractions;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Errors;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.Enumerations;
using Shopway.Infrastructure.Payments.DummyGatewayTypes;
using Shopway.Infrastructure.Payments.DummyGatewayTypes.Events;
using Shopway.Infrastructure.Payments.DummyGatewayTypes.Refunds;
using Shopway.Infrastructure.Payments.DummyGatewayTypes.Sessions;

namespace Shopway.Infrastructure.Payments;

public sealed partial class PaymentGatewayService(IHttpContextAccessor httpContextAccessor, IOptions<PaymentGatewayOptions> options)
    : IPaymentGatewayService
{
    private const string BaseUrl = "http://localhost:8080/api/OrderHeaders/payment";
    private const string SuccessUrl = $"{BaseUrl}/success";
    private const string CancelUrl = $"{BaseUrl}/cancel";
    private const string PaymentMode = "payment";
    private const string Currency = "PLN";
    private const string SignatureHeader = "PaymentGateway-Signature";
    private const string LocationHeader = "Location";

    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly PaymentGatewayOptions _paymentGatewayOptions = options.Value;

    public async Task<Result<Domain.Orders.ValueObjects.Session>> StartSessionAsync(OrderHeader orderHeader)
    {
        var httpResponse = _httpContextAccessor.HttpContext?.Response;

        if (httpResponse is null)
        {
            return Result.Failure<Domain.Orders.ValueObjects.Session>(Error.NullReference(nameof(httpResponse)));
        }

        var options = new SessionCreateOptions
        {
            LineItems = orderHeader.OrderLines.Select(ToPaymentGatewayItem).ToList(),
            Mode = PaymentMode,
            SuccessUrl = SuccessUrl,
            CancelUrl = CancelUrl,
            Currency = Currency
        };

        var service = new SessionService();
        Session session = await service.CreateAsync(options);

        httpResponse!.Headers.Append(LocationHeader, session.Url);
        return Domain.Orders.ValueObjects.Session.Create(session.Id, session.ClientSecret, session.PaymentIntentId);
    }

    public async Task<Result<(string SessionId, PaymentStatus PaymentStatus)>> GetPaymentProcessResult()
    {
        var httpRequest = _httpContextAccessor.HttpContext?.Request;

        if (httpRequest is null)
        {
            return Result.Failure<(string SessionId, PaymentStatus PaymentStatus)>(Error.NullReference(nameof(httpRequest)));
        }

        string webhookSecret = _paymentGatewayOptions.WebhookSecret!;

        try
        {
            return await GetPaymentGatewayEvent(httpRequest, webhookSecret) switch
            {
                { Type: Events.CheckoutSessionCompleted, Data.Object: Session session } 
                    => Result.Success((session.Id, PaymentStatus.Confirmed)),

                { Type: Events.CheckoutSessionAsyncPaymentSucceeded, Data.Object: Session session } 
                    => Result.Success((session.Id, PaymentStatus.Received)),

                { Type: Events.CheckoutSessionAsyncPaymentFailed, Data.Object: Session session } 
                    => Result.Success((session.Id, PaymentStatus.Failed)),

                _ => Result.Failure<(string SessionId, PaymentStatus PaymentStatus)>(Error.NotSupported("Event type not supported")),
            };
        }
        catch (PaymentGatewayException exception)
        {
            return Result.Failure<(string SessionId, PaymentStatus PaymentStatus)>(Error.FromException(exception));
        }
    }

    public async Task<Result> Refund(Domain.Orders.ValueObjects.Session session)
    {
        var options = new RefundCreateOptions
        {
            PaymentIntent = session.PaymentIntentId
        };

        var service = new RefundService();

        try
        {
            await service.CreateAsync(options);
        }
        catch (PaymentGatewayException exception)
        {
            return Result.Failure(Error.FromException(exception));
        }

        return Result.Success();
    }

    private SessionLineItemOptions ToPaymentGatewayItem(OrderLine line)
    {
        return new SessionLineItemOptions()
        {
            Quantity = line.Amount.Value,
            PriceData = new SessionLineItemPriceDataOptions()
            {
                Currency = Currency,
                ProductData = new SessionLineItemPriceDataProductDataOptions()
                {
                    Name = line.ProductSummary.ProductName.Value
                },
                UnitAmountDecimal = line.ProductSummary.Price.Value
            }
        };
    }

    private static async Task<PaymentGatewayEvent> GetPaymentGatewayEvent(HttpRequest httpRequest, string webhookSecret)
    {
        var json = await new StreamReader(httpRequest.Body).ReadToEndAsync();
        return PaymentGatewayEventUtility.ConstructEvent(json, httpRequest.Headers[SignatureHeader], webhookSecret);
    }
}
