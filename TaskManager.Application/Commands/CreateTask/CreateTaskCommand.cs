using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using TaskManager.Application.DTOs; 

namespace TaskManager.Application.Commands.CreateTask
{
    public record CreateTaskCommand(string Title, string? Description) : IRequest<TaskDto>;
}
