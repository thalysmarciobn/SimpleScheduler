using System;
using SimpleScheduler.Scheduler;

namespace sample1
{
    public record TestJob : Job
    {
        private int Id { get; }
        public TestJob(int id)
        {
            Id = id;
        }
        public override Task ExecuteAsync()
        {
            Console.WriteLine($"{Id} : {DateTime.Now:hh:mm:ss}");
            return Task.CompletedTask;
        }
    }
}