using DevSync.Contexts;
using DevSync.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DevSync.Features.Projects.Queries;

public class GetAllUserProjectsHandler : IRequestHandler<GetAllUserProjectsQuery, IEnumerable<Project>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllUserProjectsHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Project>> Handle(GetAllUserProjectsQuery request, CancellationToken cancellationToken) => 
        await _dbContext.Project
            .Where(project => project.Members.Any(x => x.UserId == request.UserId))
            .ToListAsync(cancellationToken);
    
}

