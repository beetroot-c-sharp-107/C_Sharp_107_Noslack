using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.UserControllerDTO
{
    public class GetUserDTO
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public Guid? AvatarFileGuid { get; set; }
        public DateTime? LastSeen { get; set; }
    }
}
