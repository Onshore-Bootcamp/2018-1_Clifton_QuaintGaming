using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace QuaintGaming.Models
{
    public class CommentPO
    {
        public int CommentID { get; set; }

        [DisplayName("Date and time")]
        public DateTime CommentTime { get; set; }

        //Accepts input string as a multiline text.
        [Required(ErrorMessage = "Text is required to post a comment or rating")]
        [StringLength(1000, ErrorMessage = "A comment cannot exceed 1,000 characters")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Comment Text")]
        public string CommentText { get; set; }

        //Only accepts the byte values between 0 and 5.
        [Range(0,5)]
        public byte Rating { get; set; }

        public string Username { get; set; }
        
        public string GameName { get; set; }

        [Required]
        public int GameID { get; set; }

        [Required]
        public int UserID { get; set; }
    }
}