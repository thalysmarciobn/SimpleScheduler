using System;

namespace SimpleScheduler.Scheduler
{
    public class JobAction
    {
        public JobAction(Job job, JobList<JobAction> list)
        {
            if (job is null) throw new ArgumentNullException(nameof(job));
            Action = job;
            State = JobState.Waiting;
            DateTime = DateTime.Now;
            JobScheduled = new JobScheduled(this, list);
        }

        public Job Action { get; }

        public JobState State { get; set; }

        public bool Repeat { get; set; } = false;

        public DateTime DateTime { get; set; }

        public JobScheduled JobScheduled { get; }

        public long Milliseconds { get; set; } = 0;

        public void AddMilliseconds(int count)
        {
            DateTime = DateTime.AddMilliseconds(count);
        }
    }
}