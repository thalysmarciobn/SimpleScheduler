using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SimpleScheduler.Scheduler;

namespace SimpleScheduler
{
    public class ScheduleManager
    {
        private readonly JobList<JobAction> _jobs = new JobList<JobAction>();

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private readonly List<Task> _workerTasks = new List<Task>();

        private int Timer { get; }

        public int Count => _jobs.Count;

        public int Running => _jobs.List.Count(x => x.State == JobState.Running);

        public int Waiting => _jobs.List.Count(x => x.State == JobState.Waiting);

        private bool Alive { get; set; }

        public ScheduleManager(int threads = 1, int timer = 500)
        {
            Alive = true;
            Timer = timer;
            InitializeWorkerThreads(threads);
        }

        private void InitializeWorkerThreads(int threads)
        {
            for (int i = 0; i < threads; i++)
            {
                var workerTask = Task.Run(() => Execute(), _cancellationTokenSource.Token);
                _workerTasks.Add(workerTask);
            }
        }

        public void Abort()
        {
            Alive = false;
            _jobs.Clear();
            _cancellationTokenSource.Cancel();
            Task.WhenAll(_workerTasks).Wait();
            _cancellationTokenSource.Dispose();
        }

        private async Task Execute()
        {
            while (Alive)
            {
                ProcessFinalizingJobs();
                await ProcessWaitingJobsAsync();
                Thread.Sleep(Timer);
            }
        }

        private void ProcessFinalizingJobs()
        {
            _jobs.List.RemoveAll(x =>
            {
                if (x.State == JobState.Finalizing)
                {
                    x.Action.Inmate();
                    return true;
                }
                return false;
            });
        }

        private async Task ProcessWaitingJobsAsync()
        {
            foreach (var job in _jobs.List.Where(x => x.State == JobState.Waiting && x.DateTime <= DateTime.Now).ToList())
            {
                if (job.DateTime > DateTime.Now) return;

                job.FiredException = false;

                try
                {
                    job.State = JobState.Running;
                    await job.Action.ExecuteAsync();
                    job.State = job.Repeat ? JobState.Waiting : JobState.Finalizing;
                }
                catch (Exception exception)
                {
                    job.Action.OnException(exception);
                    job.FiredException = true;
                }

                if (job.Repeat)
                {
                    job.State = JobState.Waiting;
                    job.DateTime = job.DateTime.AddMilliseconds(job.Milliseconds);
                }
            }
        }

        public JobScheduled Schedule(Job job)
        {
            return new JobAction(job, _jobs).JobScheduled;
        }
    }
}