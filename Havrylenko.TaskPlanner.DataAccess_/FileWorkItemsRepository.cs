using Havrylenko.TaskPlanner.Domain.Models_;
using System.Collections.Generic; // Для List та Dictionary
using System.IO;                 // Для File
using System.Linq;                 // Для .ToDictionary() та .Values
using System;
using Newtonsoft.Json; // <-- Використовуємо нову бібліотеку

namespace Havrylenko.TaskPlanner.DataAccess_
{
    public class FileWorkItemsRepository : IWorkItemRepository
    {
        // 1. Назва файлу як приватна константа
        private const string _filePath = "work-items.json"; // Як у PDF

        // 2. Сховище в пам'яті - Словник (Dictionary), як вимагалось
        private readonly Dictionary<Guid, WorkItem> _itemsDictionary;

        public FileWorkItemsRepository()
        {
            // 3. Читаємо файл у конструкторі
            if (!File.Exists(_filePath))
            {
                // Якщо файлу нема, створюємо порожній словник
                _itemsDictionary = new Dictionary<Guid, WorkItem>();
                return;
            }

            string jsonString = File.ReadAllText(_filePath);

            // 4. Десеріалізуємо в масив (або List), як вимагалось
            var itemsList = JsonConvert.DeserializeObject<List<WorkItem>>(jsonString) ?? new List<WorkItem>();

            // 5. Конвертуємо масив у словник
            _itemsDictionary = itemsList.ToDictionary(
                item => item.Id,  // Ключ - це Id задачі
                item => item     // Значення - це сама задача
            );
        }

        public Guid Add(WorkItem workItem)
        {
            // 1. Створює копію об’єкту, що передано в метод (використовуємо метод Clone())
            var workItemCopy = workItem.Clone();

            // 2. Створює новий Guid і записує його в відповідне поле копії
            workItemCopy.Id = Guid.NewGuid();

            // 3. Додає копію в словник
            _itemsDictionary[workItemCopy.Id] = workItemCopy;

            // 4. Повертає створений ID
            return workItemCopy.Id;
        }

        public WorkItem? Get(Guid id)
        {
            // Пошук у словнику (миттєвий)
            _itemsDictionary.TryGetValue(id, out var item);
            return item; // Поверне item або null
        }

        public WorkItem[] GetAll()
        {
            // Повертаємо всі значення зі словника
            return _itemsDictionary.Values.ToArray();
        }

        public bool Remove(Guid id)
        {
            // Видалення зі словника (миттєве)
            return _itemsDictionary.Remove(id);
        }

        public bool Update(WorkItem workItem)
        {
            // Перевіряємо, чи такий Id взагалі існує
            if (!_itemsDictionary.ContainsKey(workItem.Id))
            {
                return false;
            }

            // Оновлюємо значення
            _itemsDictionary[workItem.Id] = workItem;
            return true;
        }

        public void SaveChanges()
        {
            // Налаштування для гарного форматування
            var options = new JsonSerializerSettings { Formatting = Formatting.Indented };

            // Беремо всі *значення* зі словника (це буде колекція WorkItem)
            // і серіалізуємо їх як масив у JSON
            string jsonString = JsonConvert.SerializeObject(_itemsDictionary.Values, options);

            File.WriteAllText(_filePath, jsonString);
        }
    }
}