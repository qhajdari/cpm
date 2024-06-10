using ProjectManagementService.Models;
using ProjectManagementService.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Services;
public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async System.Threading.Tasks.Task<IEnumerable<Project>> GetProjectsAsync()
    {
        return await _projectRepository.GetProjectsAsync();
    }

    public async System.Threading.Tasks.Task<Project> GetProjectByIdAsync(int id)
    {
        return await _projectRepository.GetProjectByIdAsync(id);
    }

    public async System.Threading.Tasks.Task AddProjectAsync(Project project)
    {
        await _projectRepository.AddProjectAsync(project);
    }

    public async System.Threading.Tasks.Task UpdateProjectAsync(Project project)
    {
        await _projectRepository.UpdateProjectAsync(project);
    }

    public async System.Threading.Tasks.Task DeleteProjectAsync(int id)
    {
        await _projectRepository.DeleteProjectAsync(id);
    }
}