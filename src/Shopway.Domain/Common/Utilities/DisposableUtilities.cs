namespace Shopway.Domain.Common.Utilities;

public static class DisposableUtilities
{
    public static ValueTask DisposeAsyncIfAvailable(this IDisposable? disposable)
    {
        if (disposable is not null)
        {
            if (disposable is IAsyncDisposable asyncDisposable)
            {
                return asyncDisposable.DisposeAsync();
            }

            disposable.Dispose();
        }

        return default;
    }
}