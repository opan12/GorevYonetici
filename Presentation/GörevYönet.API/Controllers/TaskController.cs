using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using GörevYönet.Application.Abstractions.Services;
using GörevYönet.Domain.Entitites;

using AutoMapper;
using GörevYönet.Domain.DTOs;
using Microsoft.AspNetCore.Identity;
using GörevYönet.Domain.Enum;


namespace GörevYönetici.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<User> _userManager;



        public TaskController(ITaskService taskService, UserManager<User> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;



        }
        [HttpPost("add")]
        public async Task<ActionResult> AddTask(TaskItem taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest("Invalid task data.");
            }
            var currentUserName = User.Identity?.Name;

            // Giriş yapan kullanıcının UserId'sini al
            var userId = _userManager.GetUserId(User);

            // DTO -> Entity dönüşüm ve UserId atama
            var taskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                IsCompleted =false,
                Progress = taskDto.Progress,
                CompletedDate = taskDto.CompletedDate,
                Priority = taskDto.Priority,
                UserId = userId, // Kullanıcının kimliğini ata
            };

            await _taskService.AddTask(taskItem); // Servis metodu

            return Ok();
        }

        [HttpGet("task-priorities")]
        public IActionResult GetTaskPriorities()
        {
            var priorities = Enum.GetValues(typeof(TaskPriority))
                                 .Cast<TaskPriority>()
                                 .Select(priority => new
                                 {
                                     Name = priority.ToString(),
                                     Value = (int)priority,
                                 })
                                 .ToList();

            return Ok(priorities);
        }
    
    [HttpGet("get-task/{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var taskDto = await _taskService.GetTaskById(id);
            return Ok(taskDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var userId = _userManager.GetUserId(User); // Kullanıcı kimliğini al
            var tasks = await _taskService.GetTasks();

            // Kullanıcının sadece kendi görevlerini almasını sağla
            var userTasks = tasks.Where(t => t.UserId == userId).ToList();

            return Ok(userTasks);
        }





        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] TaskItem updatedTask)
        {
            if (id != updatedTask.Id)
            {
                return BadRequest("Task ID does not match.");
            }

            var result = await _taskService.UpdateTask(updatedTask);
            if (result == null)
            {
                return NotFound("Task not found or already deleted.");
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var result = await _taskService.DeleteTask(id);
            if (!result)
            {
                return NotFound("Task not found or already deleted.");
            }

            return Ok("Task deleted successfully.");
        }

        [HttpGet("daily")]
        public async Task<IActionResult> GetDailyTasks()
        {
            var dailyTasks = await _taskService.GetTasksByDateRange(DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1));
            return Ok(dailyTasks);
        }

        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyTasks()
        {
            var now = DateTime.UtcNow;
            var startOfWeek = now.Date.AddDays(-(int)now.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);
            var weeklyTasks = await _taskService.GetTasksByDateRange(startOfWeek, endOfWeek);
            return Ok(weeklyTasks);
        }

        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyTasks()
        {
            var now = DateTime.UtcNow;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var endOfMonth = startOfMonth.AddMonths(1);
            var monthlyTasks = await _taskService.GetTasksByDateRange(startOfMonth, endOfMonth);
            return Ok(monthlyTasks);
        }

        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedTasks()
        {
            var tasks = await _taskService.GetSortedTasks();
            return Ok(tasks);
        }
        [HttpGet("task-grouping-keys")]
        public IActionResult GetTaskGroupingKeys()
        {
            var groupingKeys = Enum.GetValues(typeof(TaskGroupingKey))
                                   .Cast<TaskGroupingKey>()
                                   .Select(key => new
                                   {
                                       Name = key.ToString(),
                                       Value = (int)key
                                   })
                                   .ToList();

            return Ok(groupingKeys);
        }
    

    [HttpGet("sorted/user/{userId}")]
        public async Task<IActionResult> GetSortedTasksByUserId(string userId)
        {
            var tasks = await _taskService.GetSortedTasksByUserId(userId);
            return Ok(tasks);
        }
    }
}
