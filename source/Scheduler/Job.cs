using System;

namespace SimpleScheduler.Scheduler
{
    public abstract class Job : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual void Execute()
        {
        }

        public virtual void Inmate()
        {
        }

        public virtual void OnException(Exception exception)
        {
        }
    }
}