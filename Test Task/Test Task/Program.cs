
var tasks = new List<Func<Task>>()
{ 
    ()=>GetSleepTask(3000),
    ()=>GetSleepTask(4000),
    ()=>GetSleepTask(2000),
    ()=>GetSleepTask(1000),
    ()=>GetSleepTask(1000)
};

await Execute(tasks, 2);

Console.WriteLine("done");
Console.ReadKey();

Task GetSleepTask(int sleep) => new Task(()=>Thread.Sleep(sleep));

async Task Execute(IEnumerable<Func<Task>> taskFunctions, int maxParallel)
{
    var taskList = new List<Task>(maxParallel);
    foreach (var taskFunc in taskFunctions)
    {
        var task = taskFunc();
        taskList.Add(task);
        if (taskList.Count == maxParallel)
        {
            var idx = await Task.WhenAny(taskList).ConfigureAwait(false);
            taskList.RemoveAt(idx);
        }
    }
    await Task.WhenAll(taskList).ConfigureAwait(false);
}