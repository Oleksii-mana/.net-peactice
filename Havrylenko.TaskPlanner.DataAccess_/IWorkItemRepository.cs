using Havrylenko.TaskPlanner.Domain.Models_;

namespace Havrylenko.TaskPlanner.DataAccess_
{
    public interface IWorkItemRepository
    {
        // Метод для завантаження
        WorkItem[] LoadWorkItems();

        // Метод для збереження
        void SaveWorkItems(WorkItem[] items);
    }
}