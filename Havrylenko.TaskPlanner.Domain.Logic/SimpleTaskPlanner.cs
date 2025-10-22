using Havrylenko.TaskPlanner.Domain.Models_;
// Додаємо using для інтерфейсу репозиторію
using Havrylenko.TaskPlanner.DataAccess_;

namespace Havrylenko.TaskPlanner.Domain.Logic_ // Перевірте ваш namespace
{
    public class SimpleTaskPlanner : ITaskPlanner
    {
        // 1. Приватне поле для зберігання репозиторію
        private readonly IWorkItemRepository _repository;

        // 2. Конструктор, що приймає репозиторій (Dependency Injection)
        public SimpleTaskPlanner(IWorkItemRepository repository)
        {
            _repository = repository;
        }

        // 3. Метод CreatePlan тепер не має параметрів
        public WorkItem[] CreatePlan()
        {
            // 4. Отримуємо дані САМОСТІЙНО з репозиторію
            var items = _repository.GetAll();

            // 5. Логіка сортування залишається незмінною
            var itemsAsList = items.ToList();
            itemsAsList.Sort(CompareWorkItems);
            return itemsAsList.ToArray();
        }

        // Метод сортування незмінний
        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            if (firstItem.Priority.CompareTo(secondItem.Priority) != 0)
            {
                return secondItem.Priority.CompareTo(firstItem.Priority);
            }

            if (firstItem.DueDate.CompareTo(secondItem.DueDate) != 0)
            {
                return firstItem.DueDate.CompareTo(secondItem.DueDate);
            }

            return firstItem.Title.CompareTo(secondItem.Title);
        }
    }
}