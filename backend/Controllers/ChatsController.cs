using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using backend.DTO.ChatControllerDTO;
using backend.Models;
using backend.DTO.UserControllerDTO;
using AutoMapper.QueryableExtensions;


namespace backend.Controllers;

// GET api/chat/{id} -> { participant: {}, messages: [] }
// GET api/chat/{id}/messages -> [MessageDTO]
// GET api/chat/{id}/open -> Opens new SignalR connection
// POST api/chat -> Create a new chat with user specified in the CreateChatDTO

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
    [ProducesResponseType(typeof(GetChatDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChatById([FromRoute] int id, CancellationToken cancellationToken)
    {
        if (!Request.Cookies.ContainsKey("currentUserId"))
        {
            return BadRequest("currentUserId not present in the cookies collection.");
        }

        if (!int.TryParse(Request.Cookies["currentUserId"], out var currentUserId))
        {
            return BadRequest("currentUserId is present in the cookies collection, but has incorrect format.");
        }

        var chat = await _chatDbContext.Chats.FindAsync(id, cancellationToken);
        if (chat is null)
        {
            return NotFound(id);
        }

        if (chat.UserId1 != currentUserId && chat.UserId2 != currentUserId)
        {
            return Forbid();
        }

        int OtherParticipantId = chat.UserId2 == currentUserId ? chat.UserId1 : chat.UserId2;
        User chatParticipant = await _chatDbContext.Users.FindAsync(OtherParticipantId, cancellationToken);

        var lastMessageInChat = await _chatDbContext.Messages.Where(x => x.ChatId == id).LastOrDefaultAsync(cancellationToken);
        GetChatDTO chatDTO = new GetChatDTO
        {
            Id = chat.Id,
            ChatParticipant = _mapper.Map<User, GetUserDTO>(chatParticipant),
            LastMessage = _mapper.Map<Message, MessageDTO>(lastMessageInChat),
        };

        return Ok(chatDTO);
    }

    [HttpGet("{id}/messages")]
    [ProducesResponseType(typeof(MessageDTO[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessagesByChatId([FromRoute] int id, [FromQuery] int start = 0, [FromQuery] int count = 10, CancellationToken cancellationToken = default)
    {
        var chat = await _chatDbContext.Chats.FindAsync(id, cancellationToken);
        if (chat is null)
        {
            return NotFound(id);
        }

        var messages = (await _chatDbContext.Messages
            .Where(x => x.ChatId == id)
            .Skip(start)
            .Take(count)
            .ToListAsync(cancellationToken))
            .Select(_mapper.Map<Message, MessageDTO>)
            .ToList();

        return Ok(messages);
    }
}
