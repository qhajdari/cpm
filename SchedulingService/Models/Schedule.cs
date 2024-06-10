namespace SchedulingService.Models;
public class Schedule
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Event> Events { get; set; }
}
