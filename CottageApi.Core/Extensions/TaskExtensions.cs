using System;
using System.Threading.Tasks;

namespace CottageApi.Core.Extensions
{
	public static class TaskExtensions
	{
        public static void NotAwaited(this Task task)
        {
            task.ContinueWith(
                t =>
                {
                    Console.WriteLine(t.Exception);
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
