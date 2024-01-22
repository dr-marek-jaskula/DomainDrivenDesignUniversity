using System.Runtime.CompilerServices;

namespace Shopway.Domain.Common.Utilities;

public static class TaskUtilities
{
    public static async Task<TDestination> Select<TSource, TDestination>(this Task<TSource> task, Func<TSource, TDestination> mapping)
    {
        var result = await task.ConfigureAwait(false);
        return mapping(result);
    }

    public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this (Task<T1>, Task<T2>) tuple)
    {
        return Combine(tuple.Item1, tuple.Item2).GetAwaiter();
    }

    public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
    {
        return WhenAll(tasks).GetAwaiter();
    }

    public static TaskAwaiter<IEnumerable<T>> GetAwaiter<T>(this IEnumerable<Task<T>> tasks)
    {
        return WhenAll(tasks).GetAwaiter();
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

    private static async Task WhenAll(IEnumerable<Task> tasks)
    {
        var task = Task.WhenAll(tasks);

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
    }

    private static async Task<IEnumerable<T>> WhenAll<T>(IEnumerable<Task<T>> tasks)
    {
        var task = Task.WhenAll(tasks);

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

        return tasks.Select(x => x.Result);
    }
}