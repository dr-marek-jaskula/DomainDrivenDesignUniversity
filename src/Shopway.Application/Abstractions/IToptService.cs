﻿namespace Shopway.Application.Abstractions;

public interface IToptService
{
    (string secret, string qrCode) CreateSecret(string qrLabel, int codeLength = 6, int periodInSeconds = 30, int numberOfBits = 160);
    bool VerifyCode(string secret, string code, int codeLength = 6, int periodInSeconds = 30);
}
