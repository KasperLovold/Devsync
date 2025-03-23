using DevSync.Models;
using MediatR;

namespace DevSync.Features.Projects.Queries;

public record GetAllUserProjectsQuery(int UserId) : IRequest<IEnumerable<Project>>;
