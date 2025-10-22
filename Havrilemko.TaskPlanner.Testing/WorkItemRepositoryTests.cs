using Havrylenko.TaskPlanner.DataAccess_;
using Havrylenko.TaskPlanner.Domain.Models_;
using Havrylenko.TaskPlanner.Domain.Models_.Enums;
using System.IO;
using System.Linq;

namespace Havrilemko.TaskPlanner.Testing
{
    [TestClass]
    public class WorkItemRepositoryTests
    {
        private const string _filePath = "work-items.json";

        [TestInitialize]
        public void Setup()
        {
            // Завжди видаляємо файл перед тестом, щоб почати "з чистого аркуша"
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
        }

        [TestMethod]
        public void Add_Get_Update_Remove_And_SaveChanges_ShouldWork()
        {
            // --- ARRANGE (Test 1: Add, Get, GetAll) ---
            // Створюємо repo1. У цей момент він завантажує 0 елементів.
            IWorkItemRepository repo1 = new FileWorkItemsRepository();
            var item1 = new WorkItem("Task 1 (High)", DateTime.Now, Priority.High);
            var item2 = new WorkItem("Task 2 (Low)", DateTime.Now, Priority.Low);

            // --- ACT 1 ---
            var newId1 = repo1.Add(item1);
            var newId2 = repo1.Add(item2);

            // --- ASSERT 1 (Перевіряємо "пам'ять") ---
            Assert.AreEqual(2, repo1.GetAll().Length);
            Assert.AreEqual("Task 1 (High)", repo1.Get(newId1)?.Title);

            // --- ARRANGE 2 (Update) ---
            // Модифікуємо об'єкт (симулюємо зміну назви)
            item1.Title = "Updated Task 1";

            item1.Id = newId1;

            // --- ACT 2 ---
            repo1.Update(item1);

            // --- ASSERT 2 (Перевіряємо "пам'ять") ---
            Assert.AreEqual(2, repo1.GetAll().Length); // Кількість та сама
            Assert.AreEqual("Updated Task 1", repo1.Get(newId1)?.Title); // Назва оновилась

            // --- ARRANGE 3 (Remove) ---
            // --- ACT 3 ---
            repo1.Remove(newId2); // Видаляємо друге завдання

            // --- ASSERT 3 (Перевіряємо "пам'ять") ---
            Assert.AreEqual(1, repo1.GetAll().Length); // Залишилось одне
            Assert.IsNull(repo1.Get(newId2)); // Друге не знайдено

            // --- ARRANGE 4 (SaveChanges) ---
            // --- ACT 4 ---
            repo1.SaveChanges(); // Зберігаємо 1 елемент на диск

            // --- ARRANGE 5 (Load) ---
            // Створюємо НОВИЙ репозиторій. 
            // Він має завантажити 1 елемент з workitems.json у свою "пам'ять".
            IWorkItemRepository repo2 = new FileWorkItemsRepository();

            // --- ASSERT 5 (Перевіряємо "пам'ять" repo2) ---
            Assert.AreEqual(1, repo2.GetAll().Length);
            Assert.AreEqual("Updated Task 1", repo2.Get(newId1)?.Title);
        }
    }
}