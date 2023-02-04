namespace Shopway.Application.Abstractions.Batch;

public interface IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>
    where TBatchRequest : class, IBatchRequest
    where TBatchResponseKey : class, IBatchResponseKey
{
    /// <summary>
    /// If the input condition is true, append the error to the error list
    /// </summary>
    /// <param name="invalid">Condition that is true for invalid case</param>
    /// <param name="thenError">Error to add, if the condition is true</param>
    /// <returns>Same instance to chain the validation</returns>
    IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey> If(bool invalid, string thenError);

    /// <summary>
    /// In order to keep the validation clean, place the part of the validation in a separate method and pass it by ValidateUsing. 
    /// Chain more validation methods if is required.
    /// </summary>
    /// <param name="requestValidationMethod">Delegate that contains the validation</param>
    /// <returns>Same instance to chain the validation</returns>
    IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey> ValidateUsing(Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod);
}