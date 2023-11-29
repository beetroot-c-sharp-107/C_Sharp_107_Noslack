﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace backend.Models
{
    public class Messege
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }


        [ForeignKey(nameof(ChatId))]
        public Chat Chat { get; set; } = new Chat();

        [Column("chat_id")]
        public int ChatId { get; set; }


        [Column("messege")]
        public string Messeges { get; set; }


        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = new User();

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("sent_date")]
        public string Sent_date { get; set; }

    }
}