using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace backend;

public class MyCustomHealthCheckService : IHealthCheck
{
    private readonly ChatDbContext _dbContext;

    public MyCustomHealthCheckService(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            _ = await _dbContext.Users.CountAsync(cancellationToken);
            _ = await _dbContext.Avatars.CountAsync(cancellationToken);
            await _dbContext.Users.GroupBy(u => u.Nickname.First()).ToListAsync(cancellationToken);
            return new HealthCheckResult(HealthStatus.Healthy);
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(HealthStatus.Unhealthy, exception: ex);
        }
    }
}