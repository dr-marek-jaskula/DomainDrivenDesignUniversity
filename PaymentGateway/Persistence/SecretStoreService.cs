using Microsoft.AspNetCore.DataProtection;
using PaymentGateway.Cryptography;

namespace PaymentGateway.Persistence;

//Simplest implementation
public sealed class SecretStoreService
{
    public void SetSecretForIssuer(string issuer, string privateKey)
    {
        string path = GetSecretStoreIssuerPath(issuer);
        using var fileStream = File.Create(path);
        fileStream.Dispose();
        using StreamWriter outputFile = new(path);
        outputFile.WriteLine(privateKey);
    }

    public void SetWebhookSecretForIssuer(string issuer, string webhookSecret)
    {
        string path = GetWebhookSecretStoreIssuerPath(issuer);
        using var fileStream = File.Create(path);
        fileStream.Dispose();
        using StreamWriter outputFile = new(path);
        outputFile.WriteLine(webhookSecret);
    }

    public string GetPrivateKeyHashForIssuer(string issuer)
    {
        string path = GetSecretStoreIssuerPath(issuer);
        using StreamReader file = new(path);
        var secret = file.ReadLine()!;
        return HashUtilities.ComputeSha256Hash(secret);
    }

    public string GetWebhookSecretForIssuer(string issuer)
    {
        string path = GetWebhookSecretStoreIssuerPath(issuer);
        using StreamReader file = new(path);
        var webhookSecret = file.ReadLine()!;
        return webhookSecret;
    }

    private static string GetSecretStoreIssuerPath(string issuer)
    {
        var currentPath = Directory.GetCurrentDirectory();
        return Path.Combine(currentPath, "Persistence", $"SecretStore.{issuer}.txt");
    }

    private static string GetWebhookSecretStoreIssuerPath(string issuer)
    {
        var currentPath = Directory.GetCurrentDirectory();
        return Path.Combine(currentPath, "Persistence", $"SecretStore.Webook.{issuer}.txt");
    }
}
