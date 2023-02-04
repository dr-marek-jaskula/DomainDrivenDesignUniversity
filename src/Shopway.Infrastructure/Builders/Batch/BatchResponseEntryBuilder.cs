using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions.Batch;
using Shopway.Application.Batch;
using static Shopway.Application.Batch.BatchEntryStatus;

namespace Shopway.Infrastructure.Builders.Batch;

//BatchResponseEntryBuilder is only used for BatchResponseBuilder
//So to avoid having generic parameters in entry builder is a subclass in the generic BatchResponseBuilder
partial class BatchResponseBuilder<TBatchRequest, TBatchResponseKey>
    where TBatchRequest : class, IBatchRequest
    where TBatchResponseKey : class, IBatchResponseKey
{
    public sealed class BatchResponseEntryBuilder : IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>
    {
        private readonly TBatchRequest _request;
        private readonly TBatchResponseKey _responseKey;
        private readonly BatchEntryStatus _successStatus;
        private readonly IList<string> _errorMessages = new List<string>();

        internal BatchResponseEntryBuilder(TBatchRequest request, TBatchResponseKey responseKey, BatchEntryStatus successStatus)
        {
            _request = request;
            _responseKey = responseKey;
            _successStatus = successStatus;
        }

        public TBatchRequest Request => _request;

        public IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey> If(bool invalid, string thenError)
        {
            if (invalid is true)
            {
                _errorMessages.Add(thenError);
            }

            return this;
        }

        public IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey> ValidateUsing
        (
            Action<IBatchResponseEntryBuilder<TBatchRequest, TBatchResponseKey>, TBatchRequest> requestValidationMethod
        )
        {
            requestValidationMethod(this, _request);
            return this;
        }

        internal BatchResponseEntry ToBatchResponseEntry()
        {
            var responseStatus = _errorMessages.IsNullOrEmpty()
                ? _successStatus
                : Error;

            return new BatchResponseEntry(_responseKey, responseStatus, _errorMessages);
        }

        internal bool IsValid()
        {
            return _errorMessages.IsNullOrEmpty();
        }

        internal bool IsValidAndToInsert()
        {
            return IsValid() && _successStatus is Inserted;
        }

        internal bool IsValidAndToUpdate()
        {
            return IsValid() && _successStatus is Updated;
        }
    }
}
