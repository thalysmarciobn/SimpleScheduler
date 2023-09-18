using System;

namespace SimpleScheduler.Scheduler
{
    public abstract record Job
    {
        public virtual async Task ExecuteAsync()
        {
            await Task.CompletedTask;
        }

        public virtual void Inmate()
        {
        }

        public virtual void OnException(Exception exception)
        {
        }
    }
}