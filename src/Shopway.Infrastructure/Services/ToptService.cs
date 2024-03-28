using Shopway.Application.Abstractions;
using TwoFactorAuthNet;
using TwoFactorAuthNet.QRCoder;

namespace Shopway.Infrastructure.Services;

public sealed class ToptService : IToptService
{
    private const string Issuer = nameof(Shopway);

    public (string secret, string qrCode) CreateSecret(string qrLabel, int codeLength = 6, int periodInSeconds = 30, int numberOfBits = 160)
    {
        var twoFactorAuthorization = new TwoFactorAuth(Issuer, codeLength, periodInSeconds, Algorithm.SHA256, new QRCoderQRCodeProvider());
        var secret = twoFactorAuthorization.CreateSecret(numberOfBits);
        var qrCode = twoFactorAuthorization.GetQrCodeImageAsDataUri(qrLabel, secret);
        return (secret, qrCode);
    }

    public bool VerifyCode(string secret, string code, int codeLength = 6, int periodInSeconds = 30)
    {

        var twoFactorAuthorization = new TwoFactorAuth(Issuer, codeLength, periodInSeconds, Algorithm.SHA256);
        return twoFactorAuthorization.VerifyCode(secret, code);
    }
}