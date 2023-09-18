using sample1;
using SimpleScheduler.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace sample.Tests
{
    public class ActionTest
    {
        [Fact]
        public void Action()
        {
            var job = new TestJob(1);

            var action = new JobAction(job, new JobList<JobAction>());

            Assert.Equal(job, action.Action);
        }
    }
}
