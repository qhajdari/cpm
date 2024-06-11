using Microsoft.EntityFrameworkCore;
using ResourceManagementService.Models;

namespace ResourceManagementService.Data;
public class ResourceContext : DbContext
{
    public ResourceContext(DbContextOptions<ResourceContext> options) : base(options) { }

    public DbSet<Resource> Resources { get; set; }
     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("ResourceManagement");
    }
}