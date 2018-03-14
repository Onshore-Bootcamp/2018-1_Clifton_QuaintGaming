using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuaintGaming.Models;
using QuaintDAL.Models;
using QuaintBLL.Models;

namespace QuaintGaming.Mapping
{
    public class Mapper
    {
        //Maps the properties from Presentation to DataAccess
        public static GameDO GamePOtoDO(GamePO from)
        {
            GameDO to = new GameDO();
            to.GameID = from.GameID;
            to.GameName = from.GameName;
            to.ReleaseYear = from.ReleaseYear;
            to.Genre = from.Genre;
            to.Developer = from.Developer;
            to.Publisher = from.Publisher;
            to.Platform = from.Platform;
            to.Download = from.Download;
            to.Picture = from.Picture;
            to.Description = from.Description;
            return to;
        }

        //Map the properties from the DataAccess to the Presentation.
        public static GamePO GameDOtoPO(GameDO from)
        {
            GamePO to = new GamePO();
            to.GameID = from.GameID;
            to.GameName = from.GameName;
            to.ReleaseYear = from.ReleaseYear;
            to.Genre = from.Genre;
            to.Developer = from.Developer;
            to.Publisher = from.Publisher;
            to.Platform = from.Platform;
            to.Download = from.Download;
            to.Picture = from.Picture;
            to.Description = from.Description;
            return to;
        }

        //Make a list of the mapped objects from the DataAccess to the Presentation.
        public static List<GamePO> ListGameDOtoPO(List<GameDO> gameObjects)
        {
            List<GamePO> mappedItems = new List<GamePO>();
            foreach (GameDO item in gameObjects)
            {
                mappedItems.Add(GameDOtoPO(item));
            }
            return mappedItems;
        }

        //Map properties from the Presentation to the DataAccess.
        public static CommentDO CommentPOtoDO(CommentPO from)
        {
            CommentDO to = new CommentDO();
            to.CommentID = from.CommentID;
            to.CommentTime = from.CommentTime;
            to.CommentText = from.CommentText;
            to.Rating = from.Rating;
            to.GameID = from.GameID;
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.GameName = from.GameName;
            return to;
        }

        //Map properties from the DataAccess to the Presentation.
        public static CommentPO CommentDOtoPO(CommentDO from)
        {
            CommentPO to = new CommentPO();
            to.CommentID = from.CommentID;
            to.CommentTime = from.CommentTime;
            to.CommentText = from.CommentText;
            to.Rating = from.Rating;
            to.GameID = from.GameID;
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.GameName = from.GameName;
            return to;
        }

        //Make a list of mapped objects in the DataAccess to the Presentation.
        public static List<CommentPO> ListCommentDOtoPO(List<CommentDO> commentObjects)
        {
            List<CommentPO> mappedItems = new List<CommentPO>();
            foreach (CommentDO item in commentObjects)
            {
                mappedItems.Add(CommentDOtoPO(item));
            }
            return mappedItems;
        }

        //Map properties from the Presentation to the BusinessLogic.
        public static CommentBO CommentPOtoBO(CommentPO from)
        {
            CommentBO to = new CommentBO();
            to.CommentID = from.CommentID;
            to.CommentTime = from.CommentTime;
            to.CommentText = from.CommentText;
            to.Rating = from.Rating;
            to.GameID = from.GameID;
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.GameName = from.GameName;
            return to;
        }

        //Map properties from the DataAccess to the BusinessLogic
        public static CommentBO CommentDOtoBO(CommentDO from)
        {
            CommentBO to = new CommentBO();
            to.CommentID = from.CommentID;
            to.CommentTime = from.CommentTime;
            to.CommentText = from.CommentText;
            to.Rating = from.Rating;
            to.GameID = from.GameID;
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.GameName = from.GameName;
            return to;
        }

        //Map from the BusinessLogic to the Presentation
        public static CommentPO CommentBOtoPO(CommentBO from)
        {
            CommentPO to = new CommentPO();
            to.CommentID = from.CommentID;
            to.CommentTime = from.CommentTime;
            to.CommentText = from.CommentText;
            to.Rating = from.Rating;
            to.GameID = from.GameID;
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.GameName = from.GameName;
            return to;
        }

        //Make a list of mapped objects in the BusinessLogic to the DataAccess.
        public static List<CommentPO> ListCommentBOtoPO(List<CommentBO> commentObjects)
        {
            List<CommentPO> mappedItems = new List<CommentPO>();
            foreach (CommentBO item in commentObjects)
            {
                mappedItems.Add(CommentBOtoPO(item));
            }
            return mappedItems;
        }

        //Make a list of mapped objects from the DataAccess to the BusinessLogic.
        public static List<CommentBO> ListCommentDOtoBO(List<CommentDO> commentObjects)
        {
            List<CommentBO> mappedItems = new List<CommentBO>();
            foreach (CommentDO item in commentObjects)
            {
                mappedItems.Add(CommentDOtoBO(item));
            }
            return mappedItems;
        }

        //Map properties from the Presentation to the DataAccess.
        public static UserDO UserPOtoDO(UserPO from)
        {
            UserDO to = new UserDO();
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.Password = from.Password;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Email = from.Email;
            to.RoleID = from.RoleID;
            to.Title = from.Title;
            return to;
        }

        //Map properties from the DataAccess to the Presentation.
        public static UserPO UserDOtoPO(UserDO from)
        {
            UserPO to = new UserPO();
            to.UserID = from.UserID;
            to.Username = from.Username;
            to.Password = from.Password;
            to.FirstName = from.FirstName;
            to.LastName = from.LastName;
            to.Email = from.Email;
            to.RoleID = from.RoleID;
            to.Title = from.Title;
            return to;
        }

        //Make a list of mapped objects from the DataAccess to the Presentation.
        public static List<UserPO> ListUserDOtoPO(List<UserDO> userObjects)
        {
            List<UserPO> mappedItems = new List<UserPO>();
            foreach (UserDO item in userObjects)
            {
                mappedItems.Add(UserDOtoPO(item));
            }
            return mappedItems;
        }
    }
}