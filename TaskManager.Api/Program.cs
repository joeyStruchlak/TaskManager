// --- IMPORTS ---
using Microsoft.EntityFrameworkCore;         // EF Core database tools
using TaskManager.Application.UseCases;    // Bringing in Use Cases
using TaskManager.Domain.Interfaces;         // Bringing in Repository Interfaces
using TaskManager.Infrastructure.Data;       // Bringing in DbContext
using TaskManager.Infrastructure.Repositories; // Bringing in concrete Repository implementation

// 1. STARTUP BUILDER: Initializes the Web Application configuration engine
var builder = WebApplication.CreateBuilder(args);

// 2. REGISTER SERVICES (Dependency Injection Container)
// "Here is where we register all our tools so .NET knows how to create them automatically when needed."

builder.Services.AddControllers();           // Enables API Controllers
builder.Services.AddEndpointsApiExplorer();  // Helps build API documentation
builder.Services.AddSwaggerGen();            // Enables Swagger UI (testing web page)

// REGISTER DATABASE: Tells .NET to use SQL Server with the connection string from appsettings.json
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// REGISTER REPOSITORY: "Whenever a class asks for 'ITaskRepository', create and hand them a 'TaskRepository'."
// AddScoped means: Create ONE instance per HTTP web request, then destroy it when done.
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

// REGISTER USE CASES: Tell .NET how to inject our Application Use Cases into Controllers
builder.Services.AddScoped<CreateTaskUseCase>();
builder.Services.AddScoped<GetAllTasksUseCase>();

// 3. BUILD THE APP: Freezes the configuration and constructs the running application
var app = builder.Build();

// 4. CONFIGURE HTTP PIPELINE: Middleware that handles requests as they flow into the app
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   // Enable Swagger JSON documentation in development mode
    app.UseSwaggerUI(); // Serve the interactive Swagger web UI page
}

app.UseHttpsRedirection(); // Automatically redirect HTTP requests to secure HTTPS
app.UseAuthorization();    // Enables security/user permissions check (if configured)
app.MapControllers();      // Routes incoming web URLs directly to Controller methods

// 5. RUN: Start listening for web traffic on localhost!
app.Run();