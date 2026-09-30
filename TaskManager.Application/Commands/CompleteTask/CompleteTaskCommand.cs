using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace TaskManager.Application.Commands.CompleteTask
{
    public record CompleteTaskCommand(int TaskId) : IRequest<bool>;
}
