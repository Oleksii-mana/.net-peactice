// See https://aka.ms/new-console-template for more information
using Havrylenko.TaskPlanner.Domain.Models_.Enums;
using Havrylenko.TaskPlanner.Domain.Models_;
using System.Linq;
Console.OutputEncoding = System.Text.Encoding.UTF8;
WorkItem[] items =
        {
            new WorkItem("Write report", new DateTime(2025, 9, 20), Priority.High),
            new WorkItem("Fix bugs", new DateTime(2025, 9, 15), Priority.Medium),
            new WorkItem("Team meeting", new DateTime(2025, 9, 12), Priority.High),
            new WorkItem("Prepare slides", new DateTime(2025, 9, 18), Priority.Low),
            new WorkItem("Code review", new DateTime(2025, 9, 11), Priority.Medium),
            new WorkItem("Client call", new DateTime(2025, 9, 10), Priority.High),
            new WorkItem("Update docs", new DateTime(2025, 9, 17), Priority.Low),
            new WorkItem("Deploy app", new DateTime(2025, 9, 13), Priority.High),
            new WorkItem("Design UI", new DateTime(2025, 9, 16), Priority.Medium),
            new WorkItem("Backup DB", new DateTime(2025, 9, 14), Priority.Low)
        };
SimpleTaskPlanner planner = new SimpleTaskPlanner();

bool running = true;
while (running)
{
    // Сортуємо та виводимо
    var sortedItems = planner.CreatePlan(items.ToArray());

    Console.WriteLine("\n--- Поточний список завдань ---");
    foreach (var item in sortedItems)
    {
        Console.WriteLine($"{item.Priority,-6} {item.DueDate.ToShortDateString(),-12} {item.Title}");
    }

    Console.Write("\nБажаєте додати новий елемент масиву? (y/n): ");
    string answer = Console.ReadLine()?.Trim().ToLower();

    if (answer == "n")
    {
        running = false;
        Console.WriteLine("Програма завершена.");
    }
    else if (answer == "y")
    {
        // Зчитуємо новий елемент
        Console.Write("Введіть назву завдання: ");
        string title = Console.ReadLine();

        // Рік
        int year;
        while (true)
        {
            Console.Write("Введіть рік: ");
            if (int.TryParse(Console.ReadLine(), out year) && year > 0) break;
            Console.WriteLine("Некоректний рік. Спробуйте ще раз.");
        }

        // Місяць
        int month;
        while (true)
        {
            Console.Write("Введіть місяць (1-12): ");
            if (int.TryParse(Console.ReadLine(), out month) && month >= 1 && month <= 12) break;
            Console.WriteLine("Некоректний місяць. Спробуйте ще раз.");
        }

        // День
        int day;
        while (true)
        {
            Console.Write("Введіть число: ");
            if (int.TryParse(Console.ReadLine(), out day))
            {
                // перевіряємо чи існує дата
                try
                {
                    var testDate = new DateTime(year, month, day);
                    break;
                }
                catch
                {
                    Console.WriteLine("Такої дати не існує. Спробуйте ще раз.");
                }
            }
            else
            {
                Console.WriteLine("Некоректне число. Спробуйте ще раз.");
            }
        }

        // Пріоритет
        Priority priority;
        while (true)
        {
            Console.WriteLine("Оберіть пріоритет:");
            foreach (var pr in Enum.GetValues(typeof(Priority)))
            {
                Console.WriteLine($"- {pr}");
            }
            Console.Write("Введіть пріоритет: ");
            string priorityInput = Console.ReadLine();

            if (Enum.TryParse(priorityInput, true, out priority)) break;
            Console.WriteLine("Некоректний пріоритет. Спробуйте ще раз.");
        }

        items = items.Append(new WorkItem(title, new DateTime(year, month, day), priority)).ToArray();

        Console.WriteLine("\nНовий елемент додано.\n");
    }
    else
    {
        Console.WriteLine("Невірна відповідь. Введіть 'y' або 'n'.");
    }
}
