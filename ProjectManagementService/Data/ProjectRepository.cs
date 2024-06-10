using ProjectManagementService.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Data;
public class ProjectRepository : IProjectRepository
{
    private readonly ProjectContext _context;

    public ProjectRepository(ProjectContext context)
    {
        _context = context;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Project>> GetProjectsAsync()
    {
        return await _context.Projects.Include(p => p.Tasks).ToListAsync();
    }

    public async System.Threading.Tasks.Task<Project> GetProjectByIdAsync(int id)
    {
        return await _context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async System.Threading.Tasks.Task AddProjectAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task UpdateProjectAsync(Project project)
    {
        _context.Entry(project).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }

    public async System.Threading.Tasks.Task<bool> ProjectExistsAsync(int id)
    {
        return await _context.Projects.AnyAsync(e => e.Id == id);
    }
}
