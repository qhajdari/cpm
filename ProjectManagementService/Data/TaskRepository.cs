using ProjectManagementService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Data;
public class TaskRepository : ITaskRepository
{
    private readonly ProjectContext _context;

    public TaskRepository(ProjectContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Models.Task>> GetTasksAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async System.Threading.Tasks.Task<Models.Task> GetTaskByIdAsync(int id)
    {
        return await _context.Tasks.FindAsync(id);
    }

    public async System.Threading.Tasks.Task AddTaskAsync(Models.Task task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task UpdateTaskAsync(Models.Task task)
    {
        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }

    public async System.Threading.Tasks.Task<bool> TaskExistsAsync(int id)
    {
        return await _context.Tasks.AnyAsync(e => e.Id == id);
    }
}
