using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleScheduler.Scheduler;

namespace SimpleScheduler
{
    public class SchedulerManager
    {
        private readonly JobList<JobAction> _jobs = new JobList<JobAction>();

        public SchedulerManager(int threads = 1, int timer = 500)
        {
            _alive = true;
            _timer = timer;
            _threads = Enumerable.Range(0, threads).Select(i => new Thread(Execute)).ToArray();
            foreach (var thread in _threads) thread.Start();
        }

        private Thread[] _threads { get; }
        private int _timer { get; }
        public int Count => _jobs.Count;
        public int Running => _jobs.List().Count(x => x.State == JobState.Running);
        public int Waiting => _jobs.List().Count(x => x.State == JobState.Waiting);
        private bool _alive { get; set; }

        public void Abort()
        {
            _alive = false;
            _jobs.Clear();
            foreach (var thread in _threads) thread.Abort();
        }

        private void Execute()
        {
            while (_alive)
            {
                _jobs.List().Where(x => x.State == JobState.Finalizing).ToList().ForEach(x =>
                {
                    _jobs.Remove(x);
                    x.Action.Inmate();
                    x.Action.Dispose();
                });
                _jobs.List().Where(x => x.State == JobState.Waiting).ToList().ForEach(x =>
                {
                    if (x.DateTime > DateTime.Now) return;
                    Task.Run(() =>
                    {
                        try
                        {
                            x.State = JobState.Running;
                            x.Action.Execute();
                            x.State = x.Repeat ? JobState.Waiting : JobState.Finalizing;
                        }
                        catch (Exception exception)
                        {
                            x.Action.OnException(exception);
                        }
                    });
                    if (x.Repeat) x.DateTime = x.DateTime.AddMilliseconds(x.Milliseconds);
                });
                Thread.Sleep(_timer);
            }
        }

        public JobScheduled Schedule(Job job)
        {
            return new JobAction(job, _jobs).JobScheduled;
        }
    }
}