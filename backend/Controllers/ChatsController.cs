using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using backend.DTO.ChatControllerDTO;


namespace backend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ChatsController : ControllerBase
{
    private readonly ILogger<ChatsController> _logger;
    private readonly ChatDbContext _chatDbContext;
    private readonly IMapper _mapper;

    public ChatsController(ILogger<ChatsController> logger, ChatDbContext chatDbContext, IMapper mapper)
    {
        _logger = logger;
        _chatDbContext = chatDbContext;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ChatById(int id, CancellationToken cancellationToken)
    {
        var chat = await _chatDbContext.Chats.FindAsync(id, cancellationToken);


        if (chat is null)
        {
            return NotFound(id);
        }
        return Ok(_mapper.Map<Chat, ChatDTO>(chat));
    }

    [HttpGet("{id}/messages")]
    public async Task<IActionResult> GetMessagesById(int id, CancellationToken cancellationToken)
    {
        return Ok();
    }
}
