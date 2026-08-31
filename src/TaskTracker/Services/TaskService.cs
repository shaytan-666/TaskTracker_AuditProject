using TaskTracker.Models;

namespace TaskTracker.Services;

public class TaskService
{
    private readonly List<TaskItem> _items = new();

    public void Add(TaskItem item) => _items.Add(item);

    public IReadOnlyList<TaskItem> GetAll() => _items;
}
