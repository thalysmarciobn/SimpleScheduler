namespace test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var job = new TestJob(1);

            var action = new JobAction(job, new JobList<JobAction>());

            Assert.Equal(job, action.Action);
        }
    }

    public record TestJob : Job
    {
        private int Id { get; }
        public TestJob(int id)
        {
            Id = id;
        }
        public override Task ExecuteAsync()
        {
            Console.WriteLine($"{Id} : {DateTime.Now:hh:mm:ss}");
            return Task.CompletedTask;
        }
    }
}