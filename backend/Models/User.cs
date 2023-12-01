using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nickname { get; set; }


        [Column("password")]
        [MaxLength(256)]
        public string PasswordHash { get; set; }


        [ForeignKey(nameof(AvatarId))]
        public Avatar? Avatar { get; set; } = new Avatar();


        [Column("avatar_id")]
        public int? AvatarId { get; set; }


        [Column("last_seen")]
        public DateTime LastSeen { get; set; }
    }
}
