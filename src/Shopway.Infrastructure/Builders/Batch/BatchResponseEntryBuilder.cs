using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions.Batch;
using Shopway.Application.Batch;
using static Shopway.Application.Batch.BatchEntryStatus;

namespace Shopway.Infrastructure.Builders.Batch;

//BatchResponseEntryBuilder is only used for BatchResponseBuilder
//So to avoid having generic parameters in this entry builder, it is a subclass in the generic BatchResponseBuilder
partial class BatchResponseBuilder<TBatchRequest, TBatchResponseKey>
    where TBatchRequest : class, IBatchRequest
    where TBatchResponseKey : class, IBatchResponseKey
{
    /// <summary>
    /// Builder used to build a single response entry from a single request, with possible errors and result status 
    /// </summary>
    public sealed class BatchResponseEntryBuilder : IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>
    {
        private readonly TBatchRequest _request;
        
        /// <summary>
        /// The key, that represents the request uniqueness. Usually the unique composed key, created by few entity properties
        /// </summary>
        private readonly TBatchResponseKey _responseKey;

        /// <summary>
        /// Status that will be used, if validation succeeds. If not, the 'Error' status will be used instead.
        /// </summary>
        private readonly BatchEntryStatus _successStatus;

        /// <summary>
        /// Error messages. If there is no errors, then the validation succeeds and the success status is used.
        /// </summary>
        private readonly IList<string> _errorMessages;

        internal BatchResponseEntryBuilder(TBatchRequest request, TBatchResponseKey responseKey, BatchEntryStatus successStatus)
        {
            _request = request;
            _responseKey = responseKey;
            _successStatus = successStatus;
            _errorMessages = new List<string>();
        }

        /// <summary>
        /// Request used to create the ResponseKey and being validated to inspect errors.
        /// </summary>
        public TBatchRequest Request => _request;
        internal bool IsValid => _errorMessages.IsNullOrEmpty();
        internal bool IsValidAndToInsert => IsValid && _successStatus is Inserted;
        internal bool IsValidAndToUpdate => IsValid && _successStatus is Updated;

        /// <summary>
        /// Validate the provided condition and add an error, if it is true
        /// </summary>
        /// <param name="invalid">Condition representing the invalid case</param>
        /// <param name="thenError">Error that will be added to error list if invalid condition is true</param>
        /// <returns></returns>
        public IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey> If(bool invalid, string thenError)
        {
            if (invalid is true)
            {
                _errorMessages.Add(thenError);
            }

            return this;
        }

        /// <summary>
        /// In order to place validation in part (in separate methods) use ValidateUsing and pass the respective validation part as a input delegate
        /// </summary>
        /// <param name="requestValidationMethod">Validation action that represent the validation that needs to be performed</param>
        /// <returns></returns>
        public IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey> ValidateUsing
        (
            Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod
        )
        {
            requestValidationMethod(this, _request);
            return this;
        }

        /// <summary>
        /// Builds the response entry based on a previous validations. If there is at least one error, status will be set to error.
        /// </summary>
        /// <returns>Response entry: (ResponseKey, ResponseStatus, ErrorMessages)</returns>
        internal BatchResponseEntry BuildBatchResponseEntry()
        {
            var responseStatus = _errorMessages.IsNullOrEmpty()
                ? _successStatus
                : Error;

            return new BatchResponseEntry(_responseKey, responseStatus, _errorMessages);
        }
    }
}
