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
    secretStore.SetSecretForIssuer(configureIssuerRequest.Issuer, configureIssuerRequest.PrivateKey);
    return TypedResults.Ok();
})
.WithName("ShareSecret")
.WithOpenApi();

app.MapPost("/configure-webhook", ([FromServices] WebhookService webhookService, [FromServices] SecretStoreService secretStore, [FromBody] ConfigureWebhookRequest configureWebhookRequest) =>
{
    var secretHashFromSecretStore = secretStore.GetPrivateKeyHashForIssuer(configureWebhookRequest.Subscription.Issuer);
    var secretHashFromRequest = configureWebhookRequest.SecretHash;

    if (secretHashFromSecretStore != secretHashFromRequest)
    {
        return Results.BadRequest("Secrets do not match");
    }

    secretStore.SetWebhookSecretForIssuer(configureWebhookRequest.Subscription.Issuer, configureWebhookRequest.Subscription.WebhookSecret);
    webhookService.Subscribe(configureWebhookRequest.Subscription);

    return TypedResults.Ok();
})
.WithName("ConfigureWebhook")
.WithOpenApi();

app.MapPost("/redirect-to-payment-session", ([FromBody] PaymentRequest paymentRequest) =>
{
    return TypedResults.Ok();
})
.WithName("RedirectToPaymentSession")
.WithOpenApi();

app.MapPost("/publish-payment-result/success", async ([FromServices] WebhookService webhookService, [FromServices] SecretStoreService secretStore, [FromBody] PublishRequest publishRequest) =>
{
    var webhookSecret = secretStore.GetWebhookSecretForIssuer(publishRequest.Issuer);
    await webhookService.PublishMessage(webhookSecret, publishRequest.Issuer, publishRequest.SessionId, publishRequest.ClientSecret, true);
    return TypedResults.Ok();
})
.WithName("PublishPaymentResult_Success")
.WithOpenApi();

app.MapPost("/publish-payment-result/failure", async ([FromServices] WebhookService webhookService, [FromServices] SecretStoreService secretStore, [FromBody] PublishRequest publishRequest) =>
{
    var webhookSecret = secretStore.GetWebhookSecretForIssuer(publishRequest.Issuer);
    await webhookService.PublishMessage(webhookSecret, publishRequest.Issuer, publishRequest.SessionId, publishRequest.ClientSecret, false);
    return TypedResults.Ok();
})
.WithName("PublishPaymentResult_Failure")
.WithOpenApi();

app.Run();
