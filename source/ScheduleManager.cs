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

        private void Execute()
        {
            while (Alive)
            {
                _jobs.List.Where(x => x.State == JobState.Finalizing).ToList().ForEach(x =>
                {
                    _jobs.Remove(x);
                    x.Action.Inmate();
                });
                _jobs.List.Where(x => x.State == JobState.Waiting).ToList().ForEach(x =>
                {
                    if (x.DateTime > DateTime.Now) return;
                    Task.Run(async () =>
                    {
                        try
                        {
                            x.State = JobState.Running;
                            await x.Action.ExecuteAsync();
                            x.State = x.Repeat ? JobState.Waiting : JobState.Finalizing;
                        }
                        catch (Exception exception)
                        {
                            x.Action.OnException(exception);
                        }
                    });
                    if (x.Repeat) x.DateTime = x.DateTime.AddMilliseconds(x.Milliseconds);
                });
                Thread.Sleep(Timer);
            }
        }

        public JobScheduled Schedule(Job job)
        {
            return new JobAction(job, _jobs).JobScheduled;
        }
    }
}