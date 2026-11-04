using System.Text;
using TaskTracker.Models;
using TaskTracker.Services;

namespace TaskTracker.Modules.Reports;

public class TaskReportService
{
    private readonly TaskService _taskService;

    public TaskReportService(TaskService taskService)
    {
        _taskService = taskService;
    }

    public string BuildSummary()
    {
        var tasks = _taskService.GetAll().ToList();

        if (tasks.Count == 0)
        {
            return "=== Отчёт по задачам ===\nЗадачи отсутствуют.";
        }

        var completed = tasks.Count(t => t.Status == TaskState.Completed);
        var completionPercent = (int)Math.Round((double)completed * 100 / tasks.Count);

        var overdue = tasks.Count(t => t.Status != TaskState.Completed
                                     && t.DueDate.HasValue
                                     && t.DueDate.Value < DateTime.Now);

        var highPriority = tasks.Count(t => t.Priority == TaskPriority.High);

        var loadedAssigneeGroup = tasks
            .Where(t => !string.IsNullOrWhiteSpace(t.Assignee))
            .GroupBy(t => t.Assignee.Trim().ToLower())
            .OrderByDescending(g => g.Count())
            .FirstOrDefault();

        var mostLoadedAssignee = loadedAssigneeGroup != null ? loadedAssigneeGroup.Key : "Не назначено";

        var builder = new StringBuilder();
        builder.AppendLine("=== Отчёт по задачам ===");
        builder.AppendLine("Всего задач: " + tasks.Count);
        builder.AppendLine("Выполнено: " + completed);
        builder.AppendLine("Процент выполнения: " + completionPercent + "%");
        builder.AppendLine("Просрочено: " + overdue);
        builder.AppendLine("Высокий приоритет: " + highPriority);
        builder.AppendLine("Самый загруженный исполнитель: " + mostLoadedAssignee);
        return builder.ToString();
    }

    public List<TaskItem> GetOverdueTasks()
    {
        return _taskService.GetAll()
            .Where(t => t.Status != TaskState.Completed
                     && t.DueDate.HasValue
                     && t.DueDate.Value < DateTime.Now)
            .ToList();
    }
}