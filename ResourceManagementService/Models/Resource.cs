namespace ResourceManagementService.Models;
public class Resource
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; } // e.g., Human, Equipment, Material
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
}

