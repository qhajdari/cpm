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
public class EventController : ControllerBase
{
    private readonly SchedulingContext _context;

    public EventController(SchedulingContext context)
    {
        _context = context;
    }

    // GET: /Event
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        return await _context.Events.ToListAsync();
    }

    // GET: /Event/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var ev = await _context.Events.FindAsync(id);

        if (ev == null)
        {
            return NotFound();
        }

        return ev;
    }

    // POST: /Event
    [HttpPost]
    public async Task<ActionResult<Event>> CreateEvent(Event ev)
    {
        _context.Events.Add(ev);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = ev.Id }, ev);
    }

    // PUT: /Event/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, Event ev)
    {
        if (id != ev.Id)
        {
            return BadRequest();
        }

        _context.Entry(ev).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Events.Any(e => e.Id == id))
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

    // DELETE: /Event/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var ev = await _context.Events.FindAsync(id);
        if (ev == null)
        {
            return NotFound();
        }

        _context.Events.Remove(ev);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}