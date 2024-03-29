using PaymentGateway.Cryptography;

namespace PaymentGateway.Persistence;

//Simplest implementation
public sealed class SecretStoreService
{
    public void SetSecretForIssuer(string issuer, string secret)
    {
        string path = GetSecretStoreIssuerPath(issuer);
        using var fileStream = File.Create(path);
        fileStream.Dispose();
        using StreamWriter outputFile = new(path);
        outputFile.WriteLine(secret);
    }

    public string GetSecretHashForIssuer(string issuer)
    {
        string path = GetSecretStoreIssuerPath(issuer);
        using StreamReader file = new(path);
        var secret = file.ReadLine()!;
        return HashUtilities.ComputeSha256Hash(secret);
    }

    private static string GetSecretStoreIssuerPath(string issuer)
    {
        var currentPath = Directory.GetCurrentDirectory();
        return Path.Combine(currentPath, "Persistence", $"SecretStore.{issuer}.txt");
    }
}
