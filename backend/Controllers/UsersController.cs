using AutoMapper;
using backend.DTO.UserControllerDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger;

    private readonly ChatDbContext _appDbContext;

    public UsersController(IMapper mapper, ChatDbContext appDbContext, ILogger<UsersController> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _appDbContext = appDbContext;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(GetUserDTO[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> AllAsync([FromQuery] int start = 0, [FromQuery] int count = 10, CancellationToken cancellationToken = default)
    {
        var all = await _appDbContext.Users
        .Skip(start)
        .Take(count)
        .ToListAsync(cancellationToken);

        return Ok(all
            .Select(
                c => _mapper.Map<User, GetUserDTO>(c)
            )
            .ToArray() //Try ToList later
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetUserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var contact = await _appDbContext.Users.FindAsync(id, cancellationToken);
        if(contact is null)
        {
            return NotFound(id);
        }
        return Ok(_mapper.Map<User, GetUserDTO>(contact));
    }
}
