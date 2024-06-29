using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;

namespace Shopway.Application.Abstractions.CQRS.Batch;

/// <summary>
/// Builder used to validate the request and then create the single response entry 
/// </summary>
/// <typeparam name="TBatchRequest">Batch Request type</typeparam>
/// <typeparam name="TResponseKey">Unique Response Key type</typeparam>
public interface IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>
    where TBatchRequest : class, IBatchRequest
    where TResponseKey : struct, IUniqueKey
{
    /// <summary>
    /// If the input condition is true, append the error to the error list
    /// </summary>
    /// <param name="condition">Condition that is true for invalid case</param>
    /// <param name="thenError">Error to add, if the condition is true</param>
    /// <returns>Same instance to chain the validation</returns>
    IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> If(bool condition, Error thenError);

    /// <summary>
    /// In order to keep the validation clean, place the part of the validation in a separate method and pass it by ValidateUsing. 
    /// Chain more validation methods if is required.
    /// </summary>
    /// <param name="requestValidationMethod">Delegate that contains the validation</param>
    /// <returns>Same instance to chain the validation</returns>
    IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> ValidateUsing(Action<IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>, TBatchRequest> requestValidationMethod);

    /// <summary>
    /// Use the predefined value object validation method called "Validate" to get possible validation errors
    /// </summary>
    /// <typeparam name="TValueObject">Type of value object</typeparam>
    /// <param name="parameters">Parameters for value object validation method</param>
    /// <returns>Same instance to chain the validation</returns>
    IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> UseValueObjectValidation<TValueObject>(params object[] parameters)
            where TValueObject : IValueObject;
}
