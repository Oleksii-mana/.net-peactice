using Havrylenko.TaskPlanner.Domain.Models_;
using System.Text.Json; // <-- Наш новий інструмент
using System.IO; // <-- Для роботи з файлами

namespace Havrylenko.TaskPlanner.DataAccess_
{
    public class WorkItemRepository : IWorkItemRepository
    {
        // Визначаємо шлях до нашого файлу. 
        // Він буде лежати в тій же папці, де і .exe файл програми.
        private const string _filePath = "workitems.json";

        public WorkItem[] LoadWorkItems()
        {
            // Перевіряємо, чи файл взагалі існує
            if (!File.Exists(_filePath))
            {
                // Якщо ні - повертаємо порожній масив
                return new WorkItem[0];
            }

            // Читаємо весь текст з файлу
            string jsonString = File.ReadAllText(_filePath);

            // Десеріалізуємо (перетворюємо) текст JSON назад у масив об'єктів
            var items = JsonSerializer.Deserialize<WorkItem[]>(jsonString);

            return items ?? new WorkItem[0];
        }

        public void SaveWorkItems(WorkItem[] items)
        {
            // Налаштування для гарного форматування JSON файлу (щоб його було зручно читати)
            var options = new JsonSerializerOptions { WriteIndented = true };

            // Серіалізуємо (перетворюємо) масив об'єктів у текст JSON
            string jsonString = JsonSerializer.Serialize(items, options);

            // Записуємо весь текст у файл, перезаписуючи старий
            File.WriteAllText(_filePath, jsonString);
        }
    }
}