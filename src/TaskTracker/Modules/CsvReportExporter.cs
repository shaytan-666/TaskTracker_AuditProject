using System.Text;
using TaskTracker.Models;

namespace TaskTracker.Modules.Reports;

public class CsvReportExporter
{
    public void Export(IEnumerable<TaskItem> tasks)
    {
        var relativePath = Path.Combine("data", "reports", "tasks.csv");

        try
        {
            var directory = Path.GetDirectoryName(relativePath);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var lines = new List<string>
            {
                "Id,Title,Status,Priority,Assignee,DueDate"
            };

            foreach (var task in tasks)
            {
                var dueDateStr = task.DueDate.HasValue ? task.DueDate.Value.ToString("yyyy-MM-dd") : "";

                lines.Add(string.Join(",",
                    task.Id,
                    EscapeCsvField(task.Title),
                    task.Status,
                    task.Priority,
                    EscapeCsvField(task.Assignee),
                    dueDateStr));
            }

            File.WriteAllLines(relativePath, lines, Encoding.UTF8);
            Console.WriteLine("Отчёт успешно сохранён: " + relativePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при экспорте в CSV: " + ex.Message);
        }
    }

    private string EscapeCsvField(string field)
    {
        if (string.IsNullOrEmpty(field)) return "\"\"";
        if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}