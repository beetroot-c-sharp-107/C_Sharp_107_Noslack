using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.UserControllerDTO
{
    public class CreateUserDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nickname { get; set; }

        [Required]
        [MaxLength(256)]
        public string PasswordHash { get; set; }

        public IFormFile? AvatarFile { get; set; } 

        public DateTime LastSeen { get; set; }
    }
}
