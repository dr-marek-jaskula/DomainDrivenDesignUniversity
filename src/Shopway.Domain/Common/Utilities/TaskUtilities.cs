using System.Runtime.CompilerServices;

namespace Shopway.Domain.Common.Utilities;

public static class TaskUtilities
{
    public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this (Task<T1>, Task<T2>) tuple)
    {
        return Combine(tuple.Item1, tuple.Item2).GetAwaiter();
    }

    private static async Task<(T1, T2)> Combine<T1, T2>(Task<T1> task1, Task<T2> task2)
    {
        var task = Task.WhenAll(task1, task2);

        try
        {
            await task;
        }
        catch (Exception)
        {
            if (task.Exception is not null)
            {
                throw task.Exception;
            }
            throw;
        }

        return (task1.Result, task2.Result);
    }
}