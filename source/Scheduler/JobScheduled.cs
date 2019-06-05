using System;

namespace SimpleScheduler.Scheduler
{
    public class JobScheduled
    {
        public JobScheduled(JobAction action, JobList<JobAction> jobList)
        {
            Action = action;
            List = jobList;
        }

        private JobAction Action { get; }

        private JobList<JobAction> List { get; }

        public JobScheduled AddTime(TimeSpan fisrt)
        {
            Action.Milliseconds += (int) fisrt.TotalMilliseconds;
            Action.AddMilliseconds((int) fisrt.TotalMilliseconds);
            return this;
        }

        public JobScheduled AddTime(TimeSpan fisrt, TimeSpan second)
        {
            Action.Milliseconds += (int) second.TotalMilliseconds;
            Action.AddMilliseconds((int) fisrt.TotalMilliseconds);
            return this;
        }

        public JobScheduled Repeat()
        {
            Action.Repeat = true;
            return this;
        }

        public bool Build()
        {
            lock (List)
            {
                return List.TryAdd(Action);
            }
        }

        public bool Remove()
        {
            lock (List)
            {
                return List.Remove(Action);
            }
        }
    }
}