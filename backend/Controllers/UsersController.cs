using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger;

    private readonly AppDbContext _appDbContext;

    public UsersController(IMapper mapper, AppDbContext appDbContext, ILogger<UsersController> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _appDbContext = appDbContext;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(UserDTO[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> AllAsync([FromQuery] int start = 0, [FromQuery] int count = 10, CancellationToken cancellationToken = default)
    {
        var all = await _appDbContext.Users.ToListAsync(cancellationToken);
        return Ok(all
            .Skip(start)
            .Take(count)
            .Select(
                c => _mapper.Map<User, UserDTO>(c)
            )
            .ToArray() //Try ToList later
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetContactDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var contact = await _appDbContext.Users.FindAsync(id, cancellationToken);
        if(contact is null)
        {
            return NotFound(id);
        }
        return Ok(_mapper.Map<User, UserDTO>(contact);
    }

    [HttpGet("{id}/avatar")]
    [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAvatarAsync([FromRoute] int id, CancellationToken cancellationToken)
    {
        var avatar = await _appDbContext.Contacts
            .Include(c => c.Avatar)
            .SingleAsync(c => c.Id == id, cancellationToken);

        if (avatar.Id is null || avatar.UserId is null)
        {
            return NotFound(id);
        }
        return File(avatar.ImageData, avatar.ImageType); // review Data Models later
    }
}
