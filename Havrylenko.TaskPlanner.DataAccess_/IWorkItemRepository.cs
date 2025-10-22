using Havrylenko.TaskPlanner.Domain.Models_;
using System;

namespace Havrylenko.TaskPlanner.DataAccess_
{
    public interface IWorkItemRepository
    {
        Guid Add(WorkItem workItem);
        WorkItem? Get(Guid id);
        WorkItem[] GetAll();
        bool Update(WorkItem workItem);
        bool Remove(Guid id);
        void SaveChanges();
    }
}