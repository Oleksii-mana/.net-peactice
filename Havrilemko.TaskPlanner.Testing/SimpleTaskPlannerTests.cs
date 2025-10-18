
using Havrylenko.TaskPlanner.Domain.Models_.Enums;
using Havrylenko.TaskPlanner.Domain.Models_;
using Havrylenko.TaskPlanner.Domain.Logic_;
//using Havrylenko.TaskPlanner.Domain.Logic_;

namespace Havrilemko.TaskPlanner.Testing
{
    [TestClass]
    public class SimpleTaskPlannerTests
    {
        [TestMethod]
        public void CreatePlan_ShouldSortItems_ByPriority_ThenByDueDate_ThenByTitle()
        {
            // ARRANGE (Підготовка)
            // Створюємо хаотичний набір завдань, який перевірить всі 3 правила сортування.
            var items = new WorkItem[]
            {
                // #1: Має бути третім (Medium, але пізніша дата)
                new WorkItem
                {
                    Title = "Task C (Medium, Late)",
                    Priority = Priority.Medium,
                    DueDate = DateTime.Now.AddDays(2)
                },
                
                // #2: Має бути першим (High)
                new WorkItem
                {
                    Title = "Task A (High)",
                    Priority = Priority.High,
                    DueDate = DateTime.Now.AddDays(1)
                },

                // #3: Має бути четвертим (Low)
                new WorkItem
                {
                    Title = "Task D (Low)",
                    Priority = Priority.Low,
                    DueDate = DateTime.Now.AddDays(1)
                },

                // #4: Має бути другим (Medium, але рання дата)
                new WorkItem
                {
                    Title = "Task B (Medium, Early)",
                    Priority = Priority.Medium,
                    DueDate = DateTime.Now.AddDays(1)
                }
            };

            //var planner = new SimpleTaskPlanner();

            //// ACT (Дія)
            //// Викликаємо метод, який тестуємо
            //var actualSortedItems = planner.CreatePlan(items);
            // Тепер ми тестуємо будь-яку реалізацію ITaskPlanner
            ITaskPlanner planner = new SimpleTaskPlanner();
            var actualSortedItems = planner.CreatePlan(items);

            // ASSERT (Перевірка)
            // Створюємо масив у тому порядку, в якому ми ОЧІКУЄМО отримати результат
            var expectedSortedItems = new WorkItem[]
            {
                items[1], // Task A (High)
                items[3], // Task B (Medium, Early)
                items[0], // Task C (Medium, Late)
                items[2]  // Task D (Low)
            };

            // Порівнюємо очікуваний масив з фактичним
            CollectionAssert.AreEqual(expectedSortedItems, actualSortedItems);
        }
    }
}