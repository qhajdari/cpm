using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Data;
using ProjectManagementService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementService.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ProjectContext _context;

    public TaskController(ProjectContext context)
    {
        _context = context;
    }

    // GET: /Task
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
    {
        return await _context.Tasks.ToListAsync();
    }

    // GET: /Task/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Models.Task>> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
        {
            return NotFound();
        }

        return task;
    }

    // POST: /Task
    [HttpPost]
    public async Task<ActionResult<Models.Task>> CreateTask(Models.Task task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
    }

    // PUT: /Task/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, Models.Task task)
    {
        if (id != task.Id)
        {
            return BadRequest();
        }

        _context.Entry(task).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Tasks.Any(e => e.Id == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: /Task/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

