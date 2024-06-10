using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Models;
using ProjectManagementService.Services;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectService projectService, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            _logger.LogInformation("Getting all projects");
            var projects = await _projectService.GetProjectsAsync();
            // _logger.LogInformation("Retrieved {ProjectCount} projects", projects.Count);
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            // _logger.LogInformation("Getting project with ID {id}", id);
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
            {
                _logger.LogWarning("Project with ID {Id} not found", id);
                return NotFound();
            }
            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            _logger.LogInformation("Creating a new project");
            await _projectService.AddProjectAsync(project);
            _logger.LogInformation("Created project with ID {Id}", project.Id);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, Project project)
        {
            if (await _projectService.GetProjectByIdAsync(id) == null)
            {
                _logger.LogWarning("Project with ID {Id} not found", id);
                return NotFound();
            }

            await _projectService.UpdateProjectAsync(project);
            _logger.LogInformation("Updated project with ID {Id}", id);


            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            _logger.LogInformation("Deleting project with ID {Id}", id);
            if (await _projectService.GetProjectByIdAsync(id) == null)
            {
                _logger.LogWarning("Project with ID {Id} not found", id);
                return NotFound();
            }

            await _projectService.DeleteProjectAsync(id);
            _logger.LogInformation("Deleted project with ID {Id}", id);

            return NoContent();
        }
    }
}