using GörevYönet.Domain.Entitites;
using GörevYönet.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GörevYönet.Application.Abstractions.Services
{
    public interface ITaskService
    {
        Task<TaskItem> AddTask(TaskItem task);
        Task<IEnumerable<TaskItem>> GetTasks();
        Task<TaskItem> GetTaskById(Guid id);
        Task<TaskItem> UpdateTask(Guid id, TaskItem updatedTask);
        Task<bool> DeleteTask(Guid id);
        Task<IEnumerable<TaskItem>> GetTasksByDateRange(DateTime startDate, DateTime endDate);
        Task<IEnumerable<TaskItem>> GetTasksByGroupingKeyAsync(TaskGroupingKey groupingKey, string userId);
        Task<IEnumerable<TaskItem>> GetSortedTasks();


        Task<IEnumerable<TaskItem>> GetSortedTasksByUserId(string userId);
        void CheckModifiedTaskItems();
    }
}
