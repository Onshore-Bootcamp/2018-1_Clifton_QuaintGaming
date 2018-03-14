using QuaintBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaintBLL
{
    public class QuaintBLO
    {
        public decimal RatingAverage(List<byte> ratingList)
        {
            
            decimal average = 0;
            int total = ratingList.Sum(x => x);
            if (total != 0)
            {
                //Take the list of ratings where the rating is not 0, and count them
                //Divide them to a decimal
                average = Decimal.Divide(total, ratingList.Where(x => x != default(byte)).Count());
            }
            
            return average;
        }

        public int CommentCount(List<CommentBO> commentList)
        {
            //Count the number of comments at a particular gameId
            int comments = 0;
            comments = commentList.Count();
            return comments;
        }
    }
}
