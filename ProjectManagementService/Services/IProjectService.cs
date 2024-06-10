using ProjectManagementService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Services;
public interface IProjectService
{
    System.Threading.Tasks.Task<IEnumerable<Project>> GetProjectsAsync();
    System.Threading.Tasks.Task<Project> GetProjectByIdAsync(int id);
    System.Threading.Tasks.Task AddProjectAsync(Project project);
    System.Threading.Tasks.Task UpdateProjectAsync(Project project);
    System.Threading.Tasks.Task DeleteProjectAsync(int id);
}