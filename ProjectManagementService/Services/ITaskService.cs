using ProjectManagementService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Services;
public interface ITaskService
{
    System.Threading.Tasks.Task<IEnumerable<Models.Task>> GetTasksAsync();
    System.Threading.Tasks.Task<Models.Task> GetTaskByIdAsync(int id);
    System.Threading.Tasks.Task AddTaskAsync(Models.Task task);
    System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task);
    System.Threading.Tasks.Task DeleteTaskAsync(int id);
}
