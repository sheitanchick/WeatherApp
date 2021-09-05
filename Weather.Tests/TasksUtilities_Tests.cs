using FluentAssertions;
using System;
using System.Threading.Tasks;
using Weather.Utility;
using Xunit;

namespace Weather.Tests
{
    public class TasksUtilities_Tests
    {
        [Fact]
        public async Task GetFirstSuccessfullyExecutedTask_ThrowsExceptionWhenProvidedTaskArrayIsEmpty()
        {
            var tasks = new Task<int>[0];

            Func<Task<Task<int>>> func = async () => await TasksUtilities.GetFirstSuccessfullyExecutedTask(tasks);

            await func.Should().ThrowAsync<ArgumentException>(); 
        }

        [Fact]
        public async Task GetFirstSuccessfullyExecutedTask_ReturnsFastestTaskWhenBothAreSuccessfull()
        {
            var slow = Task.Run(async () =>
            {
                await Task.Delay(200);
                return 2;
            });

            var fast = Task.Run(async () =>
            {
                await Task.Delay(100);
                return 1;
            });

            var tasks = new Task<int>[2]
            {
                slow,
                fast
            };

            var first = await TasksUtilities.GetFirstSuccessfullyExecutedTask(tasks);

            first.Should().Be(fast);
        }

        [Fact]
        public async Task GetFirstSuccessfullyExecutedTask_ReturnsSecondFastestTaskWhenFirstWasNotSuccessfull()
        {
            var fast = Task.Run(() => Task.FromException<int>(new Exception()));

            var slow = Task.Run(async () =>
            {
                await Task.Delay(200);
                return 2;
            });

            var tasks = new Task<int>[2]
            {
                fast,
                slow
            };

            var first = await TasksUtilities.GetFirstSuccessfullyExecutedTask(tasks);

            first.Should().Be(slow);
        }

        [Fact]
        public async Task GetFirstSuccessfullyExecutedTask_ThrowsExceptionWhenAllTasksFailed()
        {
            var fast = Task.Run(() => Task.FromException<int>(new Exception()));

            var slow = Task.Run(() => Task.FromException<int>(new Exception()));

            var tasks = new Task<int>[2]
            {
                fast,
                slow
            };

            Func<Task<Task<int>>> func = async () => await TasksUtilities.GetFirstSuccessfullyExecutedTask(tasks);

            await func.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
