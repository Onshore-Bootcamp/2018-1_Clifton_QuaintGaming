using System;

namespace QuaintDAL.Models
{
    public class GameDO
    {
        public int GameID { get; set; }

        public string GameName { get; set; }

        public short ReleaseYear { get; set; }

        public string Genre { get; set; }

        public string Developer { get; set; }

        public string Publisher { get; set; }

        public string Platform { get; set; }

        public string Download { get; set; }

        public string Picture { get; set; }

        public string Description { get; set; }
    }
}
