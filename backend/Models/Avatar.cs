﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Avatar
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }


        [ForeignKey(nameof(UserId))]
        public User? User { get; set; } = new User();

        [Column("user_id")]
        public int? UserId { get; set; }


        [Column("file_path")]
        public string File_path { get; set; }
    }
}