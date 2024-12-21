using GörevYönet.Domain.Enum;
using System;

namespace GörevYönet.Domain.DTOs
{
    public class TaskItemDTO
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public double Progress { get; set; }
        public DateTime? CompletedDate { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
