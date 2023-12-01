using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace backend.DTO.UserControllerDTO
{
    public class CreateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string Nickname { get; set; }

        [Required]
        [MaxLength(256)]
        public string Password { get; set; }

        public IFormFile? AvatarFile { get; set; }
    }
}
