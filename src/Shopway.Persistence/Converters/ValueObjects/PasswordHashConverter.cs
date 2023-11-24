﻿using Shopway.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Shopway.Persistence.Converters.ValueObjects;

public sealed class PasswordHashConverter : ValueConverter<PasswordHash, string>
{
    public PasswordHashConverter() : base(passwordHash => passwordHash.Value, @string => PasswordHash.Create(@string).Value) { }
}

public sealed class PasswordHashComparer : ValueComparer<PasswordHash>
{
    public PasswordHashComparer() : base((passwordHash1, passwordHash2) => passwordHash1!.Value == passwordHash2!.Value, passwordHash => passwordHash.GetHashCode()) { }
}