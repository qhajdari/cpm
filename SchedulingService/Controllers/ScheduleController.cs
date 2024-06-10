using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulingService.Data;
using SchedulingService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulingService.Controllers;
[ApiController]
[Route("[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly SchedulingContext _context;
    private readonly ILogger<ScheduleController> _logger;

    public ScheduleController(SchedulingContext context, ILogger<ScheduleController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Schedule
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
    {
          _logger.LogInformation("Getting all schedules");
        return await _context.Schedules.Include(s => s.Events).ToListAsync();
    }

    // GET: /Schedule/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Schedule>> GetSchedule(int id)
    {
        var schedule = await _context.Schedules.Include(s => s.Events).FirstOrDefaultAsync(s => s.Id == id);

        if (schedule == null)
        {
            return NotFound();
        }

        return schedule;
    }

    // POST: /Schedule
    [HttpPost]
    public async Task<ActionResult<Schedule>> CreateSchedule(Schedule schedule)
    {
        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
    }

    // PUT: /Schedule/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(int id, Schedule schedule)
    {
        if (id != schedule.Id)
        {
            return BadRequest();
        }

        _context.Entry(schedule).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Schedules.Any(e => e.Id == id))
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

    // DELETE: /Schedule/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(int id)
    {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }

        _context.Schedules.Remove(schedule);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
