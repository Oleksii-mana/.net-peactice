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
        //public DateTime DueDate;
        //public Priority Priority;
        //public Complexity Complexity;
        //public string Title;
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        private string Description;
        private bool IsCompleted;

        public WorkItem(string title, DateTime dueDate, Priority priority) {
            Title = title;
            DueDate = dueDate;
            Priority = priority;
            //Complexity = complexity;
        }

        public WorkItem()
        {
        }
        public override bool Equals(object? obj)
        {
            if (obj is not WorkItem other)
            {
                return false;
            }

            return this.Title == other.Title
                && this.Priority == other.Priority
                && this.DueDate == other.DueDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Priority, DueDate);
        }
        public override string ToString()
        {
            // Повертаємо рядок з інтерполяцією, форматуванням дати і нижнім регістром для пріоритету
            return $"{Title}: due {DueDate:dd.MM.yyyy}, {Priority.ToString().ToLower()} priority";
        }
        //    public string ToString() { return Title + ": due " + DueDate + ", " + Priority + " priority"; }
    }
       


}
