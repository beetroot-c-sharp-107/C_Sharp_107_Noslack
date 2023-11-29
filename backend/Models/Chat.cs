using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;

namespace backend.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey(nameof(UserId_1))]
        public User User_1 { get; set; } = new User();

        [Column("userid_1")]
        public int UserId_1 { get; set; }


        [ForeignKey(nameof(UserId_2))]
        public User User_2 { get; set; } = new User();

        [Column("userid_2")]
        public int UserId_2 { get; set; }

    }
}
