using ProjectManagementService.Models;
using ProjectManagementService.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Services;
public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Models.Task>> GetTasksAsync()
    {
        return await _taskRepository.GetTasksAsync();
    }

    public async System.Threading.Tasks.Task<Models.Task> GetTaskByIdAsync(int id)
    {
        return await _taskRepository.GetTaskByIdAsync(id);
    }

    public async System.Threading.Tasks.Task AddTaskAsync(Models.Task task)
    {
        await _taskRepository.AddTaskAsync(task);
    }

    public async System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task)
    {
        await _taskRepository.UpdateTaskAsync(task);
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
    {
        await _taskRepository.DeleteTaskAsync(id);
    }
}