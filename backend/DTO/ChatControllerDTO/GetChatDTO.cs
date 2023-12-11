using System;
using backend.DTO.UserControllerDTO;

namespace backend.DTO.ChatControllerDTO;

public class GetChatDTO
{
	public int Id { get; set; }
	public GetUserDTO ChatParticipant { get; set; }
	public MessageDTO LastMessage { get; set; }
}
