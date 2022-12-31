using Shopway.Domain.Results;

namespace Shopway.Infrastructure.Validators;

public sealed class Validator : ValidatorBase<Validator>
{
    public Validator() : base()
    {
    }

    public Result Success()
    {
        if (IsValid)
        {
            return Result.Success();
        }

        throw new InvalidOperationException($"Validator is in invalid state: '{CreateResult().Error.Message}'");
    }

    public Result Failure()
    {
        if (IsInvalid)
        {
            return Result.Failure(CreateResult().Error);
        }

        throw new InvalidOperationException($"Validator is in valid state");
    }
}

public sealed class Validator<TValue> : ValidatorBase<Validator<TValue>>
{
    public Validator() : base() 
    {
    }

    public Result<TValue> Success(TValue value) 
    { 
        if (IsValid)
        {
            return Result.Success(value);
        }

        throw new InvalidOperationException($"Validator is in invalid state: '{CreateResult().Error.Message}'");
    }

    public Result Failure()
    {
        if (IsInvalid)
        {
            return Result.Failure(CreateResult().Error);
        }

        throw new InvalidOperationException($"Validator is in valid state");
    }
}