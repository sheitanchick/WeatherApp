using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Weather.Infrastructure.Utility
{
    public static class TasksUtilities
    {
        public static async Task<Task<T>> GetFirstSuccessfullyExecutedTask<T>(this Task<T>[] tasks)
        {
            var first = await Task.WhenAny(tasks);

            while (first.Status != TaskStatus.RanToCompletion)
            {
                tasks = tasks.Except(new[] { first }).ToArray();

                if (!tasks.Any())
                    throw new InvalidOperationException("All tasks have failed!");

                first = await Task.WhenAny(tasks);
            }

            return first;
        }

        public static CancellationTokenSource CreateLinkedTokenSource(this CancellationToken ct)
        {
            var cts = new CancellationTokenSource();

            return CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);
        }
    }
}
