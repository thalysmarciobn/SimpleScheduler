using System;
using SimpleScheduler.Scheduler;

namespace sample1
{
    public class TestJob : Job
    {
        private int Id { get; }
        public TestJob(int id)
        {
            Id = id;
        }
        public override void Execute()
        {
            Console.WriteLine($"{Id} : {DateTime.Now:hh:mm:ss}");
        }
    }
}