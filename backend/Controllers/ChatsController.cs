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
        var chat = await _chatDbContext.Chats.FindAsync(id, cancellationToken);
        if (chat is null)
        {
            return NotFound(id);
        }
        User chatParticipant;
        if (Request.Cookies.ContainsKey("currentUserId"))
        {
            var _currentUserId = Request.Cookies["currentUserId"];
        }


        if (chat.User1Id == _currentUserId)
        {
            chatParticipant = chat.User1;
        }
        else
        {
            chatParticipant = chat.User2;
        }

        var lastMessageInChat = await _chatDbContext.Messages.Where(x => x.ChatId == id).LastOrDefaultAsync(cancellationToken);
        GetChatDTO chatDTO = new GetChatDTO
        {
            Id = chat.Id,
            ChatParticipant = _mapper.Map<User, GetUserDTO>(chatParticipant),
            LastMessage = _mapper.Map<Message, MessageDTO>(lastMessageInChat),
        };

        return Ok(_mapper.Map<Chat, GetChatDTO>(chat));
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
