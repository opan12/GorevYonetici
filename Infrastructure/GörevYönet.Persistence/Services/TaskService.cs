using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using GörevYönet.Application.Abstractions.Services;
using GörevYönet.Domain.Entitites;
using GörevYönet.Domain.Enum;
using GörevYönet.Persistence.Context;
using AutoMapper;

namespace GörevYönet.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;


        public TaskService(ApplicationDbContext context)
        {
            _context = context;

        }

        public async Task<TaskItem> AddTask(TaskItem taskItem)
        {


            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }
        public async Task<IEnumerable<TaskItem>> GetTasks()
        {
            return await _context.TaskItems
                .Where(t => !t.IsDeleted)
                .ToListAsync();
        }

        public async Task<TaskItem> GetTaskById(Guid id)
        {
            return await _context.TaskItems
.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }
        public async Task<TaskItem> UpdateTask(Guid id, TaskItem updatedTask)
        {
            var existingTask = await _context.TaskItems.FindAsync(id);
            if (existingTask == null || existingTask.IsDeleted)
            {
                return null;
            }

            existingTask.Description = updatedTask.Description;
            existingTask.DueDate = updatedTask.DueDate;
            existingTask.Priority = updatedTask.Priority;
            existingTask.GroupingKey = updatedTask.GroupingKey;
            existingTask.IsCompleted = updatedTask.IsCompleted;
            existingTask.Progress = updatedTask.Progress;

            await _context.SaveChangesAsync();

            return existingTask;
        }


        public async Task<bool> DeleteTask(Guid id)
        {
            var existingTask = await _context.TaskItems.FindAsync(id);
            if (existingTask == null || existingTask.IsDeleted)
            {
                return false; 
            }

            existingTask.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByDateRange(DateTime startDate, DateTime endDate)
        {
            return await _context.TaskItems
                .Where(t => !t.IsDeleted && t.DueDate >= startDate && t.DueDate < endDate)
                .ToListAsync();
        }
        public void CheckModifiedTaskItems()
        {
            var modifiedEntries = _context.ChangeTracker.Entries<TaskItem>()
                                          .Where(e => e.State == EntityState.Modified)
                                          .ToList();

            foreach (var entry in modifiedEntries)
            {
                var modifiedEntity = entry.Entity;
                var properties = entry.OriginalValues.Properties;

                Console.WriteLine($"Entity {modifiedEntity.Id} değiştirildi.");
                foreach (var property in properties)
                {
                    if (entry.Property(property.Name).IsModified)
                    {
                        var originalValue = entry.Property(property.Name).OriginalValue;
                        var currentValue = entry.Property(property.Name).CurrentValue;
                        Console.WriteLine($"Property {property.Name}   {originalValue} den {currentValue} değişti ");
                    }

                }

            }
        }

        public Task<TaskItem> UpdateTask(TaskItem updatedTask)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskItem>> GetTasksByGroupingKeyAsync(TaskGroupingKey groupingKey, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskItem>> GetSortedTasks()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskItem>> GetSortedTasksByUserId(string userId)
        {
            throw new NotImplementedException();
        }

    
    }
}
