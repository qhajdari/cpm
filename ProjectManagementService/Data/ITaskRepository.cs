using ProjectManagementService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Data;
public interface ITaskRepository
{
    System.Threading.Tasks.Task<IEnumerable<Models.Task>> GetTasksAsync();
    System.Threading.Tasks.Task<Models.Task> GetTaskByIdAsync(int id);
    System.Threading.Tasks.Task AddTaskAsync(Models.Task task);
    System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task);
    System.Threading.Tasks.Task DeleteTaskAsync(int id);
    System.Threading.Tasks.Task<bool> TaskExistsAsync(int id);
}
