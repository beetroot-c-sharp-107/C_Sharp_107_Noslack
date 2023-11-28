using System;
using System.ComponentModel.DataAnnotations;

namespace backend.DTO.UserControllerDTO

public class AvatarDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    [MaxLength(255)]
    public string FilePath { get; set; }
}