using sample1;
using SimpleScheduler;
using System;
using System.Threading;

Console.WriteLine("Test");
var schedule = new ScheduleManager(1, 500);
schedule.Schedule(new TestJob(1)).AddTime(TimeSpan.FromSeconds(1)).Repeat().Build();
schedule.Schedule(new TestJob(2)).AddTime(TimeSpan.FromSeconds(2)).Repeat().Build();

while (true)
    Thread.Sleep(1000);