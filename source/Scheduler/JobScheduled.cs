using System;

namespace SimpleScheduler.Scheduler
{
    public class JobScheduled
    {
        private readonly JobAction _action;
        private readonly JobList<JobAction> _jobList;

        public JobScheduled(JobAction action, JobList<JobAction> jobList)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _jobList = jobList ?? throw new ArgumentNullException(nameof(jobList));
        }

        public JobScheduled AddTime(TimeSpan timeSpan)
        {
            int milliseconds = (int)timeSpan.TotalMilliseconds;
            _action.Milliseconds += milliseconds;
            _action.AddMilliseconds(milliseconds);
            return this;
        }

        public JobScheduled Repeat()
        {
            _action.Repeat = true;
            return this;
        }

        public bool Build()
        {
            lock (_jobList)
            {
                return _jobList.TryAdd(_action);
            }
        }

        public bool Remove()
        {
            lock (_jobList)
            {
                return _jobList.Remove(_action);
            }
        }
    }
}