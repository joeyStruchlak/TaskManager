using MediatR;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Queries.GetAllProjects;

// No input needed — we're just fetching everything
// IRequest<IEnumerable<ProjectDto>> = returns a list of ProjectDtos
public record GetAllProjectsQuery() : IRequest<IEnumerable<ProjectDto>>;