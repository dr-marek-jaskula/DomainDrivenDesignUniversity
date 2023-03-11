using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Application.CQRS;
using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Utilities;
using Shopway.Domain.ValueObjects;
using System.Reflection;
using static Shopway.Application.CQRS.BatchEntryStatus;

namespace Shopway.Infrastructure.Builders.Batch;

//BatchResponseEntryBuilder is only used for BatchResponseBuilder
//So to avoid having generic parameters in this entry builder, it is a subclass in the generic BatchResponseBuilder
partial class BatchResponseBuilder<TBatchRequest, TResponseKey>
    where TBatchRequest : class, IBatchRequest
    where TResponseKey : struct, IBusinessKey
{
    /// <summary>
    /// Builder used to build a single response entry from a single request, with possible errors and result status 
    /// </summary>
    public sealed class BatchResponseEntryBuilder : IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>
    {
        private readonly TBatchRequest _request;
        
        /// <summary>
        /// The key, that represents the request uniqueness. Usually the unique composed key, created by few entity properties
        /// </summary>
        private readonly TResponseKey _responseKey;

        /// <summary>
        /// Status that will be used, if validation succeeds. If not, the 'Error' status will be used instead.
        /// </summary>
        private readonly BatchEntryStatus _successStatus;

        /// <summary>
        /// Error messages. If there is no errors, then the validation succeeds and the success status is used.
        /// </summary>
        private readonly List<string> _errorMessages;

        internal BatchResponseEntryBuilder(TBatchRequest request, TResponseKey responseKey, BatchEntryStatus successStatus)
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
        /// <returns>Same instance to be able to chain validation methods</returns>
        public IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> If(bool invalid, string thenError)
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
        /// <returns>Same instance to be able to chain validation methods</returns>
        public IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> ValidateUsing
        (
            Action<IBatchResponseEntryBuilder<TBatchRequest, TResponseKey>, TBatchRequest> requestValidationMethod
        )
        {
            requestValidationMethod(this, _request);
            return this;
        }

        /// <summary>
        /// Use the public, static validation method called "Validate", defined in a given ValueObject type 
        /// </summary>
        /// <typeparam name="TValueObject">ValueObject type that we use to validate input parameters</typeparam>
        /// <param name="parameteres">Parameters that are required and sufficient to create a ValueObject</param>
        /// <returns>Same instance to be able to chain validation methods</returns>
        /// <exception cref="ArgumentException">Thrown if no parameters were specified</exception>
        /// <exception cref="InvalidOperationException">Thrown if given ValueObject type does not contain the public, static method "Validate"</exception>
        public IBatchResponseEntryBuilder<TBatchRequest, TResponseKey> UseValueObjectValidation<TValueObject>(params object[] parameteres)
            where TValueObject : ValueObject
        {
            if (parameteres.IsNullOrEmpty())
            {
                throw new ArgumentException($"There need to be at least one parameter for the validation method");
            }

            if (IsAnyNullParameter<TValueObject>(parameteres))
            {
                return this;
            }

            var validationMethod = typeof(TValueObject)
                .GetMethod("Validate", BindingFlags.Public | BindingFlags.Static);

            if (validationMethod is null)
            {
                throw new InvalidOperationException($"ValueObject: {nameof(TValueObject)} does not contain public, static method \"Validate\"");
            }

            object errors = validationMethod.Invoke(null, parameteres)!;

            var stringErrors = ((List<Shopway.Domain.Errors.Error>)errors).Select(error => error.Message);
            _errorMessages.AddRange(stringErrors);
            return this;
        }

        /// <summary>
        /// Examines if any of input parameters is null
        /// </summary>
        /// <typeparam name="TValueObject">ValueObject</typeparam>
        /// <param name="parameteres">Input parameters</param>
        /// <returns>True if there is null parameter. Otherwise, return false</returns>
        private bool IsAnyNullParameter<TValueObject>(object[] parameteres) where TValueObject : ValueObject
        {
            if (parameteres.Any(parameter => parameter is null))
            {
                _errorMessages.Add($"At least one of {typeof(TValueObject).Name} components is null");
                return true;
            }

            return false;
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
