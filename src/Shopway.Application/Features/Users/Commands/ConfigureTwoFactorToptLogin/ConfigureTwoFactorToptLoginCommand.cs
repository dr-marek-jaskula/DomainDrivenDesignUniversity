﻿using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.ConfigureTwoFactorToptLogin;

public sealed record ConfigureTwoFactorToptLoginCommand : ICommand<TwoFactorToptResponse>;
