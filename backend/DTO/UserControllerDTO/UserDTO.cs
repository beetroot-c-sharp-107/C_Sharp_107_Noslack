using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTO.UserControllerDTO

public class UserDTO
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nickname { get; set; }

    [Required]
    [MaxLength(256)]
    public string PasswordHash { get; set; }

    public int? AvatarId { get; set; }
    public AvatarDTO Avatar { get; set; }

    public DateTime LastSeen { get; set; }
}
