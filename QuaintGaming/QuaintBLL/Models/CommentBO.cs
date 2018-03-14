using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaintBLL.Models
{
    public class CommentBO
    {
        public int CommentID { get; set; }

        public DateTime CommentTime { get; set; }

        public string CommentText { get; set; }

        public byte Rating { get; set; }

        public string Username { get; set; }

        public string GameName { get; set; }

        public int GameID { get; set; }

        public int UserID { get; set; }
    }
}
