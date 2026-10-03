// --- IMPORTS ---
using FluentValidation;
using TaskManager.Application.Commands.CreateTask;
using MediatR;
using Microsoft.EntityFrameworkCore;         // EF Core database tools
using TaskManager.Application.Behaviours;
using TaskManager.Api.Middleware;
using Serilog;
using Temporalio.Extensions.Hosting;
using TaskManager.Infrastructure.Workflows;
using TaskManager.Infrastructure.Activities;
using TaskManager.Application.Queries.GetAllTasks;
using TaskManager.Domain.Interfaces;         // Bringing in Repository Interfaces
using TaskManager.Infrastructure.Data;       // Bringing in DbContext
using TaskManager.Infrastructure.Repositories; // Bringing in concrete Repository implementation

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/taskmanager-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
    .CreateLogger();



// 1. STARTUP BUILDER: Initializes the Web Application configuration engine
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// 2. REGISTER SERVICES (Dependency Injection Container)
// "Here is where we register all our tools so .NET knows how to create them automatically when needed."

builder.Services.AddControllers();           // Enables API Controllers
builder.Services.AddEndpointsApiExplorer();  // Helps build API documentation
builder.Services.AddSwaggerGen();            // Enables Swagger UI (testing web page)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllTasksQueryHandler).Assembly));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddValidatorsFromAssembly(typeof(CreateTaskCommandValidator).Assembly);

// REGISTER DATABASE: Tells .NET to use SQL Server with the connection string from appsettings.json
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// REGISTER REPOSITORY: "Whenever a class asks for 'ITaskRepository', create and hand them a 'TaskRepository'."
// AddScoped means: Create ONE instance per HTTP web request, then destroy it when done.
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// REGISTER TEMPORAL CLIENT
// Registers ITemporalClient in DI so it can be injected anywhere.
// This is what CreateTaskCommandHandler uses to fire "start workflow" at Temporal.
builder.Services.AddTemporalClient(opts =>
{
    opts.TargetHost = "localhost:7233";
    opts.Namespace = "default";
});

// REGISTER TEMPORAL WORKER
// This is a background service that connects to the Temporal server
// and listens for workflow jobs on the "task-manager" queue.
// When Temporal has work, it sends it here via gRPC on port 7233.
builder.Services.AddHostedTemporalWorker(
        clientTargetHost: "localhost:7233",
        clientNamespace: "default",
        taskQueue: "task-manager")
    .AddWorkflow<TaskAssignmentWorkflow>()
    .AddScopedActivities<TaskAssignmentActivities>();

// Add this BEFORE var app = builder.Build();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 3. BUILD THE APP: Freezes the configuration and constructs the running application
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>(); // Add custom exception handling middleware to the pipeline>

// 4. CONFIGURE HTTP PIPELINE: Middleware that handles requests as they flow into the app
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Enable Swagger JSON documentation in development mode
    app.UseSwaggerUI(); // Serve the interactive Swagger web UI page
}


app.UseCors("AllowAngularDev");
app.UseHttpsRedirection(); // Automatically redirect HTTP requests to secure HTTPS
app.UseAuthorization();    // Enables security/user permissions check (if configured)
app.MapControllers();      // Routes incoming web URLs directly to Controller methods


// 5. RUN: Start listening for web traffic on localhost!
app.Run();