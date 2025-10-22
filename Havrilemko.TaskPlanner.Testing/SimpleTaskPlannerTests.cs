using Havrylenko.TaskPlanner.Domain.Logic_; // Перевірте namespace
using Havrylenko.TaskPlanner.Domain.Models_; // Перевірте namespace
using Havrylenko.TaskPlanner.Domain.Models_.Enums;
using Havrylenko.TaskPlanner.DataAccess_; // <-- Новий using

namespace Havrilemko.TaskPlanner.Testing
{
    [TestClass]
    public class SimpleTaskPlannerTests
    {
        // --- Наш "Фальшивий" Репозиторій для Тестування ---
        // Він реалізує інтерфейс, але лише ті методи, які нам потрібні.
        private class FakeWorkItemRepository : IWorkItemRepository
        {
            private readonly WorkItem[] _items;

            // Ми передаємо йому наш тестовий масив при створенні
            public FakeWorkItemRepository(WorkItem[] items)
            {
                _items = items;
            }

            // 1. Реалізуємо метод, який буде викликати наш SimpleTaskPlanner
            public WorkItem[] GetAll()
            {
                return _items;
            }

            // 2. Решту методів залишаємо "пустими", вони не потрібні для цього тесту
            public Guid Add(WorkItem workItem) => throw new NotImplementedException();
            public WorkItem? Get(Guid id) => throw new NotImplementedException();
            public bool Remove(Guid id) => throw new NotImplementedException();
            public bool Update(WorkItem workItem) => throw new NotImplementedException();
            public void SaveChanges() => throw new NotImplementedException();
        }
        // ---------------------------------------------------


        [TestMethod]
        public void CreatePlan_ShouldSortItems_ByPriority_ThenByDueDate_ThenByTitle()
        {
            // ARRANGE (Підготовка)
            // 1. Створюємо хаотичний набір завдань
            var items = new WorkItem[]
            {
                new WorkItem { Title = "Task C (Medium, Late)", Priority = Priority.Medium, DueDate = DateTime.Now.AddDays(2) },
                new WorkItem { Title = "Task A (High)", Priority = Priority.High, DueDate = DateTime.Now.AddDays(1) },
                new WorkItem { Title = "Task D (Low)", Priority = Priority.Low, DueDate = DateTime.Now.AddDays(1) },
                new WorkItem { Title = "Task B (Medium, Early)", Priority = Priority.Medium, DueDate = DateTime.Now.AddDays(1) }
            };

            // 2. Створюємо фальшивий репозиторій
            IWorkItemRepository fakeRepo = new FakeWorkItemRepository(items);

            // 3. Передаємо фальшивий репозиторій у конструктор
            ITaskPlanner planner = new SimpleTaskPlanner(fakeRepo);

            // ACT (Дія)
            // Викликаємо метод БЕЗ параметрів
            var actualSortedItems = planner.CreatePlan();

            // ASSERT (Перевірка)
            var expectedSortedItems = new WorkItem[]
            {
                items[1], // Task A (High)
                items[3], // Task B (Medium, Early)
                items[0], // Task C (Medium, Late)
                items[2]  // Task D (Low)
            };

            CollectionAssert.AreEqual(expectedSortedItems, actualSortedItems);
        }
    }
}