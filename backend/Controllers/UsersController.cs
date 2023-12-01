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

    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly AppDbContext _appDbContext;

    public UsersController(IWebHostEnvironment hostingEnvironment, IMapper mapper, AppDbContext appDbContext, ILogger<UsersController> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _appDbContext = appDbContext;
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet("")]
    [ProducesResponseType(typeof(UserDTO[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> AllAsync([FromQuery] int start = 0, [FromQuery] int count = 10, CancellationToken cancellationToken = default)
    {
        var all = await _appDbContext.Users
        .Skip(start)
        .Take(count)
        .ToListAsync(cancellationToken);

        return Ok(all
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
        //I'm kinda still not sure if we'll be storing images locally for server
        var imageGuid = await _appDbContext.Users
            .Include(c => c.Avatars)
            .SingleAsync(cancellationToken => c.UserId == id, cancellationToken);
        //If I've got it right, it should construct a path to the image but maybe we'll need to store image format in db
        string path = Path.Combine(_hostingEnvironment.WebRootPath, "Images/") + imageGuid.FileGuid;
        byte[] imageBytes;
        try
        {
             imageBytes = System.IO.File.ReadAllBytes(path);
        }
        catch
        {
            return NotFound(id);
        }
        //Not sure how to return content type here
        return File(imageBytes, "image/png"); // review Data Models later
    }
}
