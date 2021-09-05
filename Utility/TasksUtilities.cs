using System.Linq;
using System.Threading.Tasks;

namespace Weather.Utility
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
                    throw new System.InvalidOperationException("All providers have failed to execute requests!");

                first = await Task.WhenAny(tasks);
            }

            return first;
        }
    }
}
