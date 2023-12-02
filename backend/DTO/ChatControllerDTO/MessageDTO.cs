using System;

namespace backend.DTO.ChatControllerDTO;

public class MessageDTO
{
    public int Id { get; set; }
    public int ChatId {  get; set; }
    public string Message { get; set; }
    public int UserId {  get; set; }
    public string SentDate {  get; set; }
}
