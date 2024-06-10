using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResourceManagementService.Data;
using ResourceManagementService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResourceManagementService.Controllers;
[ApiController]
[Route("[controller]")]
public class ResourceController : ControllerBase
{
    private readonly ResourceContext _context;
    private readonly ILogger<ResourceController> _logger;


    public ResourceController(ResourceContext context, ILogger<ResourceController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Resource
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Resource>>> GetResources()
    {
          _logger.LogInformation("Getting all resources");
        return await _context.Resources.ToListAsync();
    }

    // GET: /Resource/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Resource>> GetResource(int id)
    {
        var resource = await _context.Resources.FindAsync(id);

        if (resource == null)
        {
            return NotFound();
        }

        return resource;
    }

    // POST: /Resource
    [HttpPost]
    public async Task<ActionResult<Resource>> CreateResource(Resource resource)
    {
        _context.Resources.Add(resource);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetResource), new { id = resource.Id }, resource);
    }

    // PUT: /Resource/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResource(int id, Resource resource)
    {
        if (id != resource.Id)
        {
            return BadRequest();
        }

        _context.Entry(resource).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Resources.Any(e => e.Id == id))
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

    // DELETE: /Resource/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResource(int id)
    {
        var resource = await _context.Resources.FindAsync(id);
        if (resource == null)
        {
            return NotFound();
        }

        _context.Resources.Remove(resource);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
