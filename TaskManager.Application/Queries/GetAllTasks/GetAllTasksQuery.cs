using System;
using System.Collections.Generic;
using System.Text;
using MediatR;                        // gives us IRequest
using TaskManager.Application.DTOs;   // gives us TaskDto

namespace TaskManager.Application.Queries.GetAllTasks;

// A Query is a question: "give me all tasks"
// It carries no data in — we're just asking for everything
// IRequest<IEnumerable<TaskDto>> means: "when handled, return a list of TaskDtos"
public record GetAllTasksQuery : IRequest<IEnumerable<TaskDto>>;