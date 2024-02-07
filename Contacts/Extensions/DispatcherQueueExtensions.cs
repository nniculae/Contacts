using Microsoft.UI.Dispatching;
using System.Runtime.CompilerServices;

namespace Contacts.Extensions;
public static class DispatcherQueueExtensions
{
    private static readonly bool IsHasThreadAccessPropertyAvailable = true;
    // Temporarily use this extension copied from  CommunityToolkit.WinUI because of namespaces confilicts 
    public static Task EnqueueCustomAsync(this DispatcherQueue dispatcher, Action function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        if (IsHasThreadAccessPropertyAvailable && dispatcher.HasThreadAccess)
        {
            try
            {
                function();
                return Task.CompletedTask;
            }
            catch (Exception exception)
            {
                return Task.FromException(exception);
            }
        }
        return TryEnqueueAsync(dispatcher, function, priority);
        static Task TryEnqueueAsync(DispatcherQueue dispatcher, Action function, DispatcherQueuePriority priority)
        {
            Action function2 = function;
            TaskCompletionSource<object?> taskCompletionSource = new();
            if (!dispatcher.TryEnqueue(priority, delegate
            {
                try
                {
                    function2();
                    taskCompletionSource.SetResult(null);
                }
                catch (Exception exception2)
                {
                    taskCompletionSource.SetException(exception2);
                }
            }))
            {
                taskCompletionSource.SetException(GetEnqueueException("Failed to enqueue the operation"));
            }
            return taskCompletionSource.Task;
        }
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static InvalidOperationException GetEnqueueException(string message)
    {
        return new InvalidOperationException(message);
    }
}
