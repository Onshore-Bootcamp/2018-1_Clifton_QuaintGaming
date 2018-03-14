using QuaintDAL;
using QuaintDAL.Models;
using QuaintGaming.Logging;
using QuaintGaming.Mapping;
using QuaintGaming.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace QuaintGaming.Controllers
{
    public class CommentsController : Controller
    {
        //Contructor that sets the connection string to variables for use throughout the controller.
        public CommentsController()
        {
            string connection = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _dataAccess = new CommentDAO(connection);
            _gameDataAccess = new QuaintDAO(connection);
        }

        //Connection variables.
        public CommentDAO _dataAccess;
        public QuaintDAO _gameDataAccess;

        //Comments do not have a view all as they are game specific.
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Games");
        }

        //View comments for a specific game by passing in GameID
        [HttpGet]
        public ActionResult ViewGameComments(int specificGame = default(int))
        {
            ActionResult response = null;
            try
            {
                //Access the comments if a GameID is given
                if (specificGame != 0)
                {
                    //Store the game's information, proceed if the game at given ID exists.
                    GameDO game = _gameDataAccess.GameDetails(specificGame);
                    if (game != null)
                    {
                        //Get game comments from the database, map them to presentation,
                        //pass them to the ViewModel.
                        List<CommentDO> commentList = _dataAccess.ViewGameComments(specificGame);
                        List<CommentPO> displayComments = Mapper.ListCommentDOtoPO(commentList);
                        ViewCommentPO viewComment = new ViewCommentPO(displayComments);

                        //Get and set game name.
                        TempData["GameName"] = game.GameName;

                        //Attach user information.
                        if (Session["UserID"] == null)
                        {
                            viewComment.Comment.GameID = specificGame;
                        }
                        else
                        {
                            viewComment.Comment.GameID = specificGame;
                            viewComment.Comment.UserID = (int)Session["UserID"];
                        }
                        response = View(viewComment);
                    }
                    //No game found, redirect to game index.
                    else
                    {
                        
                        response = RedirectToAction("Index", "Games");
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Games");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                response = RedirectToAction("Index", "Games");
            }
            finally { }
            return response;
        }

        //Adding comments is done on the ViewGameComments page.
        [HttpPost]
        public ActionResult AddComment(ViewCommentPO view, string ratingDropdown)
        {
            ActionResult response = null;
            //Only registered users can submit comments.
            if (ModelState.IsValid && Session["UserID"] != null && (int)Session["UserRole"] <= 3)
            {
                try
                {
                    //Parse the rating given to a byte, and set the date/time of submit.
                    view.Comment.Rating = byte.Parse(ratingDropdown);
                    view.Comment.CommentTime = DateTime.Now;
                    //Add the comment to the database and refresh the page.
                    CommentDO commentObject = Mapper.CommentPOtoDO(view.Comment);
                    _dataAccess.AddComment(commentObject);
                    response = RedirectToAction("ViewGameComments", "Comments", new { specificGame = view.Comment.GameID });
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            //If adding the comment failed, give them back their comment at that game.
            else
            {
                view.CommentList = ObtainGameComments(view.Comment.GameID);
                FillTempData(view.Comment.GameID);
                if (Session["UserID"] != null)
                {
                    view.Comment.UserID = (int)Session["UserID"];
                }
                response = View("ViewGameComments", view);
            }
            return response;
        }

        //Updating comments takes place on the ViewGameComments page.
        [HttpPost]
        public ActionResult UpdateComment(ViewCommentPO view)
        {
            ActionResult response = null;
            //Only users can update their comments.
            if (ModelState.IsValid && Session["UserID"] != null && (int)Session["UserRole"] <= 3)
            {
                try
                {
                    //Sets comment time to updated time.
                    view.Comment.CommentTime = DateTime.Now;
                    //Sets new comment information to the specific comment via SQL
                    CommentDO commentObject = Mapper.CommentPOtoDO(view.Comment);
                    _dataAccess.UpdateComment(commentObject);
                    response = RedirectToAction("ViewGameComments", "Comments", new { specificGame = view.Comment.GameID });
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //Redirects to that game's page if a problem occurs
                    response = RedirectToAction("GameDetails", "Game", new { specificGame = view.Comment.GameID });
                }
                finally { }
            }
            else
            {
                response = RedirectToAction("GameDetails", "Game", new { specificGame = view.Comment.GameID });
            }
            return response;
        }

        //Comment removal takes place on the ViewGameComments page where the comment resides.
        public ActionResult RemoveComment(int specificComment, int specificGame, int specificUser)
        {
            ActionResult response = null;
            //Only users can remove their own comments, unless they are admin, who can remove any comment.
            if (Session["UserRole"] != null && ((int)Session["UserID"] == specificUser || (int)Session["UserRole"] == 1))
            {
                try
                {
                    //Removal takes place via a javascript function that calls to this method
                    _dataAccess.RemoveComment(specificComment);
                    response = RedirectToAction("ViewGameComments", "Comments", new { specificGame });
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //Redirects to the comments page if a problem occurs
                    response = RedirectToAction("ViewGameComments", "Comments", new { specificGame });
                }
            }
            return response;
        }

        //This method is called to give the comment information back if a comment fails to add.
        private List<CommentPO> ObtainGameComments(int specificGame = default(int))
        {
            List<CommentDO> commentList = _dataAccess.ViewGameComments(specificGame);
            List<CommentPO> displayComments = Mapper.ListCommentDOtoPO(commentList);
            
            return displayComments;
        }

        //This method id called to the the game's information if a comment fails to add.
        private void FillTempData(int specificGame = default(int))
        {
            //Gets the game information and sets the temp data for a specific game.
            GameDO game = _gameDataAccess.GameDetails(specificGame);
            if (game != null)
            {
                TempData["GameName"] = game.GameName;
            }
        }
    }
}