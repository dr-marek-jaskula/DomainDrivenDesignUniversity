using Shopway.Domain.Abstractions;

namespace Shopway.Domain.EntitiesBusinessKeys;

public readonly record struct ProductKey(string ProductName, string Revision) : IBusinessKey;