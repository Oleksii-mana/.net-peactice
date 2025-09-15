using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havrylenko.TaskPlanner.Domain.Models_.Enums;

namespace Havrylenko.TaskPlanner.Domain.Models_
{
    public class SimpleTaskPlanner
    {
   
            public WorkItem[] CreatePlan(WorkItem[] items)
            {
                var itemsAsList = items.ToList();
                itemsAsList.Sort(CompareWorkItems);
                return itemsAsList.ToArray();
            }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            // 1. Порівняння за пріоритетом
            int priorityComparison = firstItem.Priority.CompareTo(secondItem.Priority);
            if (priorityComparison != 0)
                return -priorityComparison; // мінус, бо більший пріоритет має бути раніше

            // 2. Порівняння за датою
            int dateComparison = firstItem.DueDate.CompareTo(secondItem.DueDate);
            if (dateComparison != 0)
                return dateComparison;

            // 3. Порівняння за назвою
            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.OrdinalIgnoreCase);
        }

    }
}
