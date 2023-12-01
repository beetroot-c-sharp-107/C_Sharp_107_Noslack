using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace backend.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(UserId1))]
        public User User1 { get; set; } = new User();

        [Column("userid_1")]
        public int UserId1 { get; set; }


        [ForeignKey(nameof(UserId2))]
        public User User2 { get; set; } = new User();

        [Column("userid_2")]
        public int UserId2 { get; set; }

    }
}
