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
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }

        public Guid Id { get; set; }
        public string Title { get; set; }
        private string Description;
        public bool IsCompleted { get; set; }

        public WorkItem(string title, DateTime dueDate, Priority priority) {
            Id = Guid.NewGuid();
            Title = title;
            DueDate = dueDate;
            Priority = priority;
        }

        public WorkItem()
        {
            Id = Guid.NewGuid();
            Title = string.Empty;
            Description = string.Empty;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not WorkItem other)
            {
                return false;
            }
            return this.Id == other.Id;
        }

        public WorkItem Clone()
        {
            return (WorkItem)this.MemberwiseClone();
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        public override string ToString()
        {
            // Повертаємо рядок з інтерполяцією, форматуванням дати і нижнім регістром для пріоритету
            return $"{Title}: due {DueDate:dd.MM.yyyy}, {Priority.ToString().ToLower()} priority";
        }
        //    public string ToString() { return Title + ": due " + DueDate + ", " + Priority + " priority"; }
    }
       


}
