
using Havrylenko.TaskPlanner.Domain.Logic_;
using Havrylenko.TaskPlanner.Domain.Models_;
using Havrylenko.TaskPlanner.Domain.Models_.Enums;
using Havrylenko.TaskPlanner.DataAccess_; // <-- Додали новий using
using System.Collections.Generic;
using System;
using System.Linq;

internal static class Program
{
    // 1. Створюємо "справжні" об'єкти (залежності) тут, один раз
    private static readonly ITaskPlanner planner = new SimpleTaskPlanner();
    private static readonly IWorkItemRepository repository = new WorkItemRepository();

    public static void Main(string[] args)
    {
        // 2. Передаємо ОБИДВІ залежності в головний метод
        RunApp(planner, repository);
    }

    // 3. Метод тепер приймає обидва інтерфейси
    public static void RunApp(ITaskPlanner taskPlanner, IWorkItemRepository repository)
    {
        // --- ЗАВАНТАЖЕННЯ ---
        // Завантажуємо існуючі завдання з файлу
        List<WorkItem> allItems = repository.LoadWorkItems().ToList();
        Console.WriteLine($"--- Loaded {allItems.Count} existing tasks from file ---");

        // Показуємо, що завантажили (але ще не сортовані)
        foreach (var item in allItems)
        {
            Console.WriteLine($"(Loaded) {item.ToString()}");
        }
        Console.WriteLine("---------------------------------------------\n");

        // --- ДОДАВАННЯ НОВИХ ---
        List<WorkItem> newItemsFromUser = new List<WorkItem>();
        while (true)
        {
            Console.WriteLine("Enter NEW task title (or leave empty to finish adding):");
            string? title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                break;
            }

            Console.WriteLine("Enter due date (e.g., 20.10.2025):");
            DateTime dueDate;
            while (!DateTime.TryParse(Console.ReadLine(), out dueDate))
            {
                Console.WriteLine("Invalid date format. Try again (e.g., 20.10.2025):");
            }

            Console.WriteLine("Enter priority (Low, Medium, High, Urgent):");
            Priority priority;
            while (!Enum.TryParse<Priority>(Console.ReadLine(), true, out priority))
            {
                Console.WriteLine("Invalid priority. Try again (Low, Medium, High, Urgent):");
            }

            newItemsFromUser.Add(new WorkItem(title, dueDate, priority));
        }

        // --- ОБ'ЄДНАННЯ, СОРТУВАННЯ І ЗБЕРЕЖЕННЯ ---

        // Додаємо нові завдання до старого списку
        allItems.AddRange(newItemsFromUser);

        // 4. Використовуємо ІНТЕРФЕЙС сортувальника
        var finalPlan = taskPlanner.CreatePlan(allItems.ToArray());

        // 5. Використовуємо ІНТЕРФЕЙС репозиторію для збереження
        repository.SaveWorkItems(finalPlan);

        Console.WriteLine($"\n--- Your final sorted plan ({finalPlan.Length} items) is now SAVED to file ---");
        foreach (var item in finalPlan)
        {
            Console.WriteLine(item.ToString());
        }
    }
}
