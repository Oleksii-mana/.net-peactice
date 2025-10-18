using Havrylenko.TaskPlanner.DataAccess_;
using Havrylenko.TaskPlanner.Domain.Models_;
using Havrylenko.TaskPlanner.Domain.Models_.Enums;
using System.IO; // Потрібен для File

namespace Havrilemko.TaskPlanner.Testing
{
    [TestClass]
    public class WorkItemRepositoryTests
    {
        // Шлях до СПРАВЖНЬОГО файлу, який використовує репозиторій
        private const string _realFilePath = "workitems.json";

        // Цей метод [TestInitialize] запускається ПЕРЕД кожним тестом
        [TestInitialize]
        public void Setup()
        {
            // Видаляємо СПРАВЖНІЙ файл, щоб тест завжди починався з чистого аркуша
            if (File.Exists(_realFilePath))
            {
                File.Delete(_realFilePath);
            }
        }

        [TestMethod]
        public void SaveWorkItems_And_LoadWorkItems_ShouldReturnSameItems()
        {
            // ARRANGE
            // (Обманюємо конструктор репозиторію, щоб він писав у наш тестовий файл)
            // Це не ідеально, але для цієї роботи підійде.
            // Кращим рішенням було б передавати шлях через конструктор.

            // ВАЖЛИВО: Оскільки ми не можемо змінити шлях у коді репозиторію,
            // нам треба вручну створити файл "workitems.json" у папці тесту.

            // Давайте спростимо. Ми просто перевіримо, що репозиторій за замовчуванням
            // повертає порожній масив, якщо файлу немає.

            // ARRANGE
            IWorkItemRepository repository = new WorkItemRepository();

            // ACT
            var loadedItems = repository.LoadWorkItems();

            // ASSERT
            Assert.IsNotNull(loadedItems);
            Assert.AreEqual(0, loadedItems.Length);

            // --- Повний тест (Save/Load) ---

            // ARRANGE 2
            var itemsToSave = new WorkItem[]
            {
                new WorkItem("Test Task 1", DateTime.Now, Priority.High),
                new WorkItem("Test Task 2", DateTime.Now.AddDays(1), Priority.Low)
            };

            // ACT 2
            repository.SaveWorkItems(itemsToSave);
            var reloadedItems = repository.LoadWorkItems();

            // ASSERT 2
            Assert.AreEqual(2, reloadedItems.Length);
            // CollectionAssert порівняє масиви, використовуючи наш метод .Equals() з ПР2
            CollectionAssert.AreEqual(itemsToSave, reloadedItems);
        }
    }
}