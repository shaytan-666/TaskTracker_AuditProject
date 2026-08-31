using TaskTracker.Models;
using TaskTracker.Services;

Console.WriteLine("TaskTracker 1.3");
var service = new TaskService();

service.Add(new TaskItem(1, "Проверить отчёт", false));
service.Add(new TaskItem(2, "Обновить документацию", true));

foreach (var task in service.GetAll())
{
    Console.WriteLine($"{task.Id}: {task.Title} | Выполнено: {task.IsCompleted}");
}
