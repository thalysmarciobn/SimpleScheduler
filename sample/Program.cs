using System;
using SimpleScheduler;

namespace sample1
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Test");
            var schedule = new SchedulerManager(1, 500);
            schedule.Schedule(new TestJob(1)).AddTime(TimeSpan.FromSeconds(1)).Repeat().Build();
            schedule.Schedule(new TestJob(2)).AddTime(TimeSpan.FromSeconds(2)).Repeat().Build();
        }
    }
}