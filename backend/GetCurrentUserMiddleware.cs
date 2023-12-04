
using backend.DTO.UserControllerDTO;
using backend.Models;

namespace backend;

public class GetCurrentUserMiddleware
{
    private readonly ILogger<GetCurrentUserMiddleware> _logger;
    private readonly RequestDelegate _next;

    public GetCurrentUserMiddleware(ILogger<GetCurrentUserMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ChatDbContext dbContext)
    {
        _logger.LogInformation("Trying to find information about current user...");

        var userId = context.Request.Cookies.FirstOrDefault(x => x.Key.Equals("userId", StringComparison.OrdinalIgnoreCase)).Value;
        var user = await dbContext.Users.FindAsync(int.Parse(userId));

        if (user is null)
        {
            _logger.LogInformation("User with id {userId} not found in the database", userId);
        }
        else
        {
            _logger.LogInformation("User with id {userId} has been found in the database: {@user}", userId, user);
            context.Items.TryAdd(nameof(User), user);
        }

        await _next(context);
    }
}