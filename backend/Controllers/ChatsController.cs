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
 
        _logger.LogInformation($"Request to get chat by id {id}");
        if (!Request.Cookies.ContainsKey("currentUserId"))
        {
            _logger.LogError("currentUserId not present in the cookies collection");
            return BadRequest("currentUserId not present in the cookies collection.");
        }

        if (!int.TryParse(Request.Cookies["currentUserId"], out var currentUserId))
        {
            _logger.LogError("Incorrect format of cookie currentUserId");
            return BadRequest("currentUserId is present in the cookies collection, but has incorrect format.");
        }

        var chat = await _chatDbContext.Chats.FindAsync(id, cancellationToken);
        if (chat is null)
        {
            _logger.LogInformation($"Chat with id {id} not found")
            return NotFound(id);
        }

        if (chat.UserId1 != currentUserId && chat.UserId2 != currentUserId)
        {
            _logger.LogWarning($"User with id {currentUserId} isn't participant of the chat with id{id}")
            return Forbid();
        }

        int participantId = chat.UserId2 == currentUserId ? chat.UserId2 : chat.UserId1;
        User chatParticipant = await _chatDbContext.Users.FindAsync(participantId, cancellationToken);

        var lastMessageInChat = await _chatDbContext.Messages.Where(x => x.ChatId == id).LastOrDefaultAsync(cancellationToken);
        GetChatDTO chatDTO = new GetChatDTO
        {
            Id = chat.Id,
            ChatParticipant = _mapper.Map<User, GetUserDTO>(chatParticipant),
            LastMessage = _mapper.Map<Message, MessageDTO>(lastMessageInChat),
        };
        _logger.LogInformation($"Successfully ruturnes chat with id: {id}");
        return Ok(chatDTO);
    }
        
    [HttpGet("{id}/messages")]
    [ProducesResponseType(typeof(MessageDTO[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(int), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessagesByChatId([FromRoute] int id, [FromQuery] int start = 0, [FromQuery] int count = 10, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Request to get messages for chat with id {id}");

        var chat = await _chatDbContext.Chats.FindAsync(id, cancellationToken);
        if (chat is null)
        {
            _logger.LogInformation($"Chat with id {id} not found")
            return NotFound(id);
        }

        _logger.LogInformation($"Getting messages for chat with id: {id}, start: {start}, count: {count}");
        var messages = (await _chatDbContext.Messages
            .Where(x => x.ChatId == id)
            .Skip(start)
            .Take(count)
            .ToListAsync(cancellationToken))
            .Select(_mapper.Map<Message, MessageDTO>)
            .ToList();

        _logger.LogInformation($"Got {messages.Count} messages for chat with id: {id}");
        return Ok(messages);
    }
    }
}
