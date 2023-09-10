# SimpleScheduler

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
