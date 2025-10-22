using Havrylenko.TaskPlanner.Domain.Logic_;
using Havrylenko.TaskPlanner.Domain.Models_;
using Havrylenko.TaskPlanner.Domain.Models_.Enums;
using Havrylenko.TaskPlanner.DataAccess_; // Переконайтеся, що назва правильна
using System;
using System.Linq;

internal static class Program
{
    // Наші залежності, як і раніше, створюються один раз
    private static readonly IWorkItemRepository repository = new FileWorkItemsRepository(); // або WorkItemRepository
    private static readonly ITaskPlanner planner = new SimpleTaskPlanner(repository);

    public static void Main(string[] args)
    {
        // Запускаємо головний цикл програми
        RunApp(planner, repository);
    }

    public static void RunApp(ITaskPlanner taskPlanner, IWorkItemRepository repo)
    {
        Console.WriteLine("--- Welcome to Task Planner ---");
        LoadAndShowTasks(repo); // Завантажуємо і показуємо завдання на старті

        bool keepRunning = true;
        while (keepRunning)
        {
            // Показуємо меню
            Console.WriteLine("\nChoose an operation:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan (Sorts and displays the current plan)");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app (Saves changes)");

            string? choice = Console.ReadLine()?.ToUpper();

            switch (choice)
            {
                case "A":
                    HandleAddTask(repo);
                    break;
                case "B":
                    HandleBuildPlan(taskPlanner, repo);
                    break;
                case "M":
                    HandleMarkCompleted(repo);
                    break;
                case "R":
                    HandleRemoveTask(repo);
                    break;
                case "Q":
                    repo.SaveChanges();
                    Console.WriteLine("Changes saved. Quitting...");
                    keepRunning = false; // Це завершить цикл while
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    // --- Допоміжні методи для кожної опції меню ---

    private static void LoadAndShowTasks(IWorkItemRepository repo)
    {
        var allItems = repo.GetAll();
        Console.WriteLine($"--- Loaded {allItems.Length} existing tasks ---");
        if (allItems.Length == 0)
        {
            Console.WriteLine("(No tasks yet)");
            return;
        }

        foreach (var item in allItems)
        {
            Console.WriteLine(item.ToString());
        }
    }

    private static void HandleAddTask(IWorkItemRepository repo)
    {
        Console.WriteLine("Enter NEW task title:");
        string? title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Adding cancelled.");
            return;
        }

        Console.WriteLine("Enter due date (e.g., 22.10.2025):");
        DateTime dueDate;
        while (!DateTime.TryParse(Console.ReadLine(), out dueDate)) { Console.WriteLine("Invalid date format."); }

        Console.WriteLine("Enter priority (Low, Medium, High, Urgent):");
        Priority priority;
        while (!Enum.TryParse<Priority>(Console.ReadLine(), true, out priority)) { Console.WriteLine("Invalid priority."); }

        var newItem = new WorkItem(title, dueDate, priority);

        var newId = repo.Add(newItem); // Використовуємо 'Add' з ПР4
        Console.WriteLine($"--- Task Added (ID: {newId}) ---");
    }

    private static void HandleBuildPlan(ITaskPlanner taskPlanner, IWorkItemRepository repo)
    {
        var itemsToSort = repo.GetAll();
        var sortedPlan = taskPlanner.CreatePlan();

        Console.WriteLine($"\n--- Your Sorted Plan ({sortedPlan.Length} items) ---");
        foreach (var item in sortedPlan)
        {
            // Оновлюємо репозиторій відсортованим порядком (як ми робили раніше)
            repo.Update(item.Clone());
            Console.WriteLine(item.ToString());
        }
        Console.WriteLine("--- End of Plan ---");
        // Примітка: Ми не зберігаємо (SaveChanges) тут, лише показуємо.
        // Збереження відбувається тільки при виході [Q].
    }

    private static void HandleMarkCompleted(IWorkItemRepository repo)
    {
        Console.WriteLine("Enter the ID of the task to mark as completed:");
        if (!Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var item = repo.Get(id);
        if (item == null)
        {
            Console.WriteLine("Task not found.");
            return;
        }

        item.IsCompleted = true; // Оновлюємо поле
        repo.Update(item);       // Оновлюємо в "пам'яті" репозиторію

        Console.WriteLine($"--- Task '{item.Title}' marked as completed ---");
    }

    private static void HandleRemoveTask(IWorkItemRepository repo)
    {
        Console.WriteLine("Enter the ID of the task to remove:");
        if (!Guid.TryParse(Console.ReadLine(), out Guid id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var itemToRemove = repo.Get(id); // Отримуємо, щоб показати назву
        if (itemToRemove == null)
        {
            Console.WriteLine("Task not found.");
            return;
        }

        if (repo.Remove(id))
        {
            Console.WriteLine($"--- Task '{itemToRemove.Title}' removed ---");
        }
        else
        {
            Console.WriteLine("Failed to remove task.");
        }
    }
}