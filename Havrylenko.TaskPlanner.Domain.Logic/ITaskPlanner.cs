using Havrylenko.TaskPlanner.Domain.Models_;

namespace Havrylenko.TaskPlanner.Domain.Logic_
{
    public interface ITaskPlanner
    {
        WorkItem[] CreatePlan(WorkItem[] items);
    }
}