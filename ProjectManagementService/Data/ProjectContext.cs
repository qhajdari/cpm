using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Models;

namespace ProjectManagementService.Data;
public class ProjectContext : DbContext
{
    public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Models.Task> Tasks { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ProjectManagement");
    }
}
