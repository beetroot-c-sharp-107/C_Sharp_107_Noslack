using AutoMapper;
using backend.DTO.UserControllerDTO;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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

    [HttpPost("")]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status406NotAcceptable)]
    public async Task<IActionResult> CreateAsync([FromForm] CreateUserDTO userDTO, CancellationToken cancellationToken)
    {
        int? avatarId = null;
        if(userDTO.AvatarFile is not null)
        {
            //Def not the best way to handle it, but can't extract content-type header as we're getting the form data
            //and I'm not sure if using FileExtensionContentTypeProvider is beneficial in this case
            if(userDTO.AvatarFile.ContentType != "image/bmp" || 
            userDTO.AvatarFile.ContentType != "image/jpeg" ||
            userDTO.AvatarFile.ContentType != "image/png")
            {
                return BadRequest("Invalid image format, only bmp, jpg and png are supported.");
            }
            avatarId = await AddAvatarAsync(userDTO.AvatarFile, cancellationToken);
        }
        var contact = _mapper.Map<CreateUserDTO, User>(userDTO);
        contact.AvatarId = avatarId;

        var createdUser = await _appDbContext.Users.AddAsync(contact);
        await _appDbContext.SaveChangesAsync(cancellationToken);
        
        return Ok(createdUser.Entity.Id);
    }

    private async Task<int> AddAvatarAsync(IFormFile avatarFile, CancellationToken cancellationToken)
    {
        Guid avatarGuid = new Guid();
        var avatarEntity = await _appDbContext.Avatars.AddAsync(new Avatar
        {
            FileGuid = avatarGuid
        }, cancellationToken);
        string webrootImages = Path.Combine("~", "images");
        if(!Directory.Exists(webrootImages))
        {
            Directory.CreateDirectory(webrootImages);
        }
        string filepath = Path.Combine(webrootImages, avatarGuid.ToString());
        using(var stream = System.IO.File.Create(filepath))
        {
            await avatarFile.CopyToAsync(stream);
        }
        await _appDbContext.SaveChangesAsync(cancellationToken);

        return avatarEntity.Entity.Id;
    }
}
