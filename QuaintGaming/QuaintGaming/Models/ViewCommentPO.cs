using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuaintGaming.Models
{
    public class ViewCommentPO
    {
        //View Model that allows the use of a list of objects and a singular object.
        public ViewCommentPO()
        {
            CommentList = new List<CommentPO>();
            Comment = new CommentPO();
        }

        public ViewCommentPO(List<CommentPO> comment)
        {
            CommentList = comment;
            Comment = new CommentPO();
        }

        public List<CommentPO> CommentList { get; set; }
        public CommentPO Comment { get; set; }
    }
}