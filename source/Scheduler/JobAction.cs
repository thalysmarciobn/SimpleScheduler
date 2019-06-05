using System;

namespace SimpleScheduler.Scheduler
{
    public class JobAction
    {
        public JobAction(Job job, JobList<JobAction> jobList)
        {
            Action = job;
            State = JobState.Waiting;
            DateTime = DateTime.Now;
            JobScheduled = new JobScheduled(this, jobList);
            Milliseconds = 0;
            Repeat = false;
        }

        public Job Action { get; }

        public JobState State { get; set; }

        public bool Repeat { get; set; }

        public DateTime DateTime { get; set; }

        public JobScheduled JobScheduled { get; }

        public long Milliseconds { get; set; }

        public void AddMilliseconds(int count)
        {
            DateTime = DateTime.AddMilliseconds(count);
        }
    }
}