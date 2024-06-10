using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Models;
using ProjectManagementService.Services;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TaskController> _logger;

    public TaskController(ITaskService taskService, ILogger<TaskController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    // GET: /Task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
    {
        _logger.LogInformation("Getting all tasks");
        var tasks = await _taskService.GetTasksAsync();
        _logger.LogInformation("Retrieved {TaskCount} tasks", tasks.Count());
        return Ok(tasks);
    }

    // GET: /Task/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Task>> GetTask(int id)
    {
        _logger.LogInformation("Getting task with ID {Id}", id);
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            _logger.LogWarning("Task with ID {Id} not found", id);
            return NotFound();
        }
        return Ok(task);
    }

    // POST: /Task
    [HttpPost]
    public async Task<ActionResult<Models.Task>> CreateTask(Models.Task task)
    {
        _logger.LogInformation("Creating a new task");
        await _taskService.AddTaskAsync(task);
        _logger.LogInformation("Created task with ID {Id}", task.Id);
        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // PUT: /Task/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, Models.Task task)
    {
        if (await _taskService.GetTaskByIdAsync(id) == null)
        {
            _logger.LogWarning("Task with ID {Id} not found", id);
            return NotFound();
        }


        await _taskService.UpdateTaskAsync(task);
        _logger.LogInformation("Updated task with ID {Id}", id);



        return NoContent();
    }

    // DELETE: /Task/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        _logger.LogInformation("Deleting task with ID {Id}", id);
        if (await _taskService.GetTaskByIdAsync(id)==null)
        {
            _logger.LogWarning("Task with ID {Id} not found", id);
            return NotFound();
        }

        await _taskService.DeleteTaskAsync(id);
        _logger.LogInformation("Deleted task with ID {Id}", id);

        return NoContent();
    }
}
