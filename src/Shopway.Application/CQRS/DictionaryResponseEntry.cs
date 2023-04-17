using Shopway.Application.Abstractions;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.CQRS;

public sealed record DictionaryResponseEntry(Guid Id, IBusinessKey BusinessKey) : IResponse;