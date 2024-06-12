using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using ProjectManagementService.Models;
using ProjectManagementService.Services;

namespace ProjectManagementService.Events
{
    public class UserRegisteredEventHandler : BackgroundService
    {
        private readonly ILogger<UserRegisteredEventHandler> _logger;
        private readonly IProjectService _projectService;
        private readonly IConfiguration _configuration;

        public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger, IProjectService projectService, IConfiguration configuration)
        {
            _logger = logger;
            _projectService = projectService;
            _configuration = configuration;
        }

        protected override  System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:HostName"] };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "user_registered",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var user = JsonSerializer.Deserialize<User>(message);

                _logger.LogInformation("Received user registered event for user: {Email}", user.Email);

                try
                {
                    await HandleUserRegistered(user);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling user registered event for user: {Email}", user.Email);
                }
            };

            channel.BasicConsume(queue: "user_registered",
                                 autoAck: true,
                                 consumer: consumer);

            _logger.LogInformation("Started listening for user registered events");

            return System.Threading.Tasks.Task.CompletedTask;
        }

        private async  System.Threading.Tasks.Task HandleUserRegistered(User user)
        {
            // Example: Create a default project for the new user
            var project = new Project
            {
                Name = $"Default Project for {user.FirstName} {user.LastName}",
                UserId = user.Id,
                StartDate = DateTime.Now
            };

            _logger.LogInformation("Creating default project for user: {Email}", user.Email);

            await _projectService.AddProjectAsync(project);

            _logger.LogInformation("Created default project for user: {Email}", user.Email);
        }
    }
}
