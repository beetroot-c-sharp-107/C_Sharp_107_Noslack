using backend.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace backend.Hubs;
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(int fromUserId, int toUserId, string message, ChatDbContext dbContext, CancellationToken cancellationToken = default)
        {
            var chat = await dbContext.Chats.Where(x => (fromUserId == x.UserId1 || fromUserId == x.UserId2) && (toUserId == x.UserId1 || toUserId == x.UserId2)).FirstOrDefaultAsync();
            //Not sure if it's the right way to get cookies from here
            int.TryParse(Context.Features.Get<HttpContext>().Request.Cookies["currentUserId"], out int currentUserId);
            if(chat is null || currentUserId != fromUserId || currentUserId != toUserId )
            {
                throw new Exception("The chat you're trying to access doesn't exist or current user is no a participant of the chat");
            }
            var messageEntity = new Message{ChatId = chat.Id, MessageText = message, UserId = currentUserId, SentDate = DateTime.UtcNow};
            await dbContext.Messages.AddAsync(messageEntity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            //We could also send some message to the sender probably but not sure if it's needed;
            await Clients.Caller.SendAsync("MessageSentSuccess");
        }
    }
}