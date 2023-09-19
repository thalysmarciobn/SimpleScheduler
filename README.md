# SimpleScheduler

[![.NET](https://github.com/thalysmarciobn/SimpleScheduler/actions/workflows/dotnet.yml/badge.svg)](https://github.com/thalysmarciobn/SimpleScheduler/actions/workflows/dotnet.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/thalysmarciobn/simplescheduler/badge)](https://www.codefactor.io/repository/github/thalysmarciobn/simplescheduler)

Um projeto simples de processamento de taferas em multi threads em queue

```csharp
var schedule = new SchedulerManager(1, 500);
schedule.Schedule(new TestJob(1)).AddTime(TimeSpan.FromSeconds(1)).Repeat().Build();
```

```csharp
public class TestJob : Job
{
    private int Id { get; }
    public TestJob(int id)
    {
        Id = id;
    }
    public override void Execute()
    {
        Console.WriteLine($"{Id} : {DateTime.Now:hh:mm:ss}");
    }
}
```
