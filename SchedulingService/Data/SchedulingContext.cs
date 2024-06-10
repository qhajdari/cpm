using Microsoft.EntityFrameworkCore;
using SchedulingService.Models;

namespace SchedulingService.Data;
public class SchedulingContext : DbContext
{
    public SchedulingContext(DbContextOptions<SchedulingContext> options) : base(options) { }

    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Event> Events { get; set; }
}

