using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QuaintGaming.Models
{
    public class GamePO
    {
        public int GameID { get; set; }

        [Required(ErrorMessage = "The game's title is required")]
        [StringLength(100, ErrorMessage = "A game's title cannot exceed 100 characters")]
        [DisplayName("Title")]
        public string GameName { get; set; }

        [Required(ErrorMessage = "The year released is required")]
        [Range(1970, 2018)]
        [DisplayName("Release Year")]
        public short ReleaseYear { get; set; }

        [StringLength(50, ErrorMessage = "Genre cannot exceed 50 characters")]
        public string Genre { get; set; }

        [StringLength(50, ErrorMessage = "Developer cannot exceed 50 characters")]
        public string Developer { get; set; }

        [StringLength(50, ErrorMessage = "Publisher cannot exceed 50 characters")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Platform is required")]
        [StringLength(20, ErrorMessage = "Platform cannot exceed 20 characters")]
        public string Platform { get; set; }

        [StringLength(260)]
        public string Download { get; set; }

        [StringLength(260)]
        public string Picture { get; set; }

        //Accepts the input string as a multiline text.
        [Required(ErrorMessage = "A short description of the game is required")]
        [StringLength(2000, ErrorMessage = "Decription cannot exceed 2,000 characters")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        public string Description { get; set; }
    }
}