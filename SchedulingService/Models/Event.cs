namespace SchedulingService.Models;


public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }
}

