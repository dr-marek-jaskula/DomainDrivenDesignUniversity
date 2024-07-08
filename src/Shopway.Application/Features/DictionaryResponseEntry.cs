using Shopway.Application.Abstractions;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Application.Features;

/// <summary>
/// If additional information is required for dictionary response entry, than just inherit from this record (create for instance OrderDictionaryResponseEntry)
/// </summary>
/// <param name="Id"></param>
/// <param name="ResponseKey"></param>
public record DictionaryResponseEntry<TResponseKey>(Ulid Id, TResponseKey ResponseKey) : IResponse, IHasCursor
    where TResponseKey : IUniqueKey;
