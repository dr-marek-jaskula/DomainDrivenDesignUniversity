using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Persistence;
using PaymentGateway.Requests;
using PaymentGateway.Webhook;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<WebhookService>();
builder.Services.AddSingleton<SecretStoreService>();
builder.Services
    .AddHttpClient("with-api-version", x =>
    {
        x.DefaultRequestHeaders.Add("api-version", "0.1");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/share-secret", ([FromServices] SecretStoreService secretStore, [FromBody] ConfigureIssuerRequest configureIssuerRequest) =>
{
    secretStore.SetSecretForIssuer(configureIssuerRequest.Issuer, configureIssuerRequest.Secret);
    return TypedResults.Ok();
})
.WithName("ShareSecret")
.WithOpenApi();

app.MapPost("/configure-webhook", ([FromServices] WebhookService webhookService, [FromServices] SecretStoreService secretStore, [FromBody] ConfigureWebhookRequest configureWebhookRequest) =>
{
    var secretHashFromSecretStore = secretStore.GetSecretHashForIssuer(configureWebhookRequest.Subscription.Issuer);
    var secretHashFromRequest = configureWebhookRequest.SecretHash;

    if (secretHashFromSecretStore != secretHashFromRequest)
    {
        return Results.BadRequest("Secrets do not match");
    }

    webhookService.Subscribe(configureWebhookRequest.Subscription);
    return TypedResults.Ok();
})
.WithName("ConfigureWebhook")
.WithOpenApi();

app.MapPost("/redirect-to-payment-session", ([FromBody] PaymentRequest paymentRequest) =>
{
    var sessionId = RandomString(6);
    return TypedResults.Ok(sessionId);

    static string RandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
})
.WithName("RedirectToPaymentSession")
.WithOpenApi();

app.MapPost("/publish-payment-result/success", async ([FromServices] WebhookService webhookService, [FromServices] SecretStoreService secretStore, [FromBody] PublishRequest publishRequest) =>
{
    var secretHash = secretStore.GetSecretHashForIssuer(publishRequest.Issuer);
    await webhookService.PublishMessage(secretHash, publishRequest.Issuer, publishRequest.SessionId, true);
    return TypedResults.Ok();
})
.WithName("PublishPaymentResult_Success")
.WithOpenApi();

app.MapPost("/publish-payment-result/failure", async ([FromServices] WebhookService webhookService, [FromServices] SecretStoreService secretStore, [FromBody] PublishRequest publishRequest) =>
{
    var secretHash = secretStore.GetSecretHashForIssuer(publishRequest.Issuer);
    await webhookService.PublishMessage(secretHash, publishRequest.Issuer, publishRequest.SessionId, false);
    return TypedResults.Ok();
})
.WithName("PublishPaymentResult_Failure")
.WithOpenApi();

app.Run();
