using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havrylenko.TaskPlanner.Domain.Models_.Enums;

namespace Havrylenko.TaskPlanner.Domain.Models_
{
    public class WorkItem
    {
        private DateTime CreationDate;
        public DateTime DueDate;
        public Priority Priority;
        public Complexity Complexity;
        public string Title;
        private string Description;
        private bool IsCompleted;

        public WorkItem(string title, DateTime dueDate, Priority priority) {
            Title = title;
            DueDate = dueDate;
            Priority = priority;
            //Complexity = complexity;
        }
        public string ToString() { return Title + ": due " + DueDate + ", " + Priority + " priority"; }
    }
    

}
