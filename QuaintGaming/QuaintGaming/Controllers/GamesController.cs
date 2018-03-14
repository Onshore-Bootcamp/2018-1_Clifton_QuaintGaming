using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using QuaintDAL;
using QuaintDAL.Models;
using QuaintGaming.Models;
using QuaintGaming.Mapping;
using System.Configuration;
using QuaintGaming.Logging;
using QuaintBLL;
using QuaintBLL.Models;
using System.Web;
using System.IO;
using QuaintGaming.Custom;
using PagedList;

namespace QuaintGaming.Controllers
{
    public class GamesController : Controller
    {
        //Contructor that sets the connection string to variables for use throughout the controller.
        public GamesController()
        {
            string connection = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _dataAccess = new QuaintDAO(connection);
            _dataAccessC = new CommentDAO(connection);
        }

        //Connection variables.
        public QuaintDAO _dataAccess;
        public CommentDAO _dataAccessC;

        //Gets all the site's games and displays 9 games per page with Pagination.
        [HttpGet]
        public ActionResult Index(int page = 1, int pageSize = 9)
        {
            ActionResult response = null;
            try
            {
                //Get and display all the games in alphabetical order, on pages 9 games at a time.
                List<GameDO> gameList = _dataAccess.ViewGames();
                List<GamePO> displayGames = Mapper.ListGameDOtoPO(gameList);
                displayGames = displayGames.OrderBy(n => n.GameName).ToList();
                PagedList<GamePO> pageList = new PagedList<GamePO>(displayGames, page, pageSize);
                response = View(pageList);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                //Redirects to the landing page if there is an issue getting the games on the index.
                response = RedirectToAction("Index", "Home");
            }
            finally { }
            return response;
        }

        //Gets the details for a selected game.
        [HttpGet]
        public ActionResult GameDetails(int specificGame = default(int))
        {
            ActionResult response = null;
            //Creates a new business object for use with displaying the game's rating.
            QuaintBLO businessAccess = new QuaintBLO();
            //string picturePath = ConfigurationManager.AppSettings.Get("FilePath");

            //Sets variables needed for the rating average and path to rating star pictures.
            decimal average = 0;
            string path = "/Content/RatingStars/";
            if (specificGame != 0)
            {
                try
                {
                    //Get a list of all comments and sends them to the business layer to be calculated.
                    List<CommentDO> commentList = _dataAccessC.ViewGameComments(specificGame);
                    List<CommentBO> sendComments = Mapper.ListCommentDOtoBO(commentList);
                    //List of bytes for all ratings for a specific GameId.
                    List<byte> ratingList = _dataAccessC.AllRatings(specificGame);
                    //Average of all ratings for a specific game retrieved after calculations
                    average = businessAccess.RatingAverage(ratingList);
                    //Display the average in a whole number with one decimal place format, coverted to string.
                    string displayAverage = average.ToString("#.#");
                    //Displays the number of comments a specific game has, as calculated by the business layer.
                    int displayCommentCount = businessAccess.CommentCount(sendComments);

                    //Get all the details for a game from the database.
                    GameDO gameObject = _dataAccess.GameDetails(specificGame);
                    GamePO displayGame = Mapper.GameDOtoPO(gameObject);

                    //Display a star image representation of the rating average recieved from the calculation.
                    if (average < 0.4m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "0star.png");
                    }
                    else if (average >= 0.5m && average < 0.9m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "0.5star.png");
                    }
                    else if (average >= 1m && average < 1.4m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "1star.png");
                    }
                    else if (average >= 1.5m && average < 1.9m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "1.5star.png");
                    }
                    else if (average >= 2m && average < 2.4m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "2star.png");
                    }
                    else if (average >= 2.5m && average < 2.9m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "2.5star.png");
                    }
                    else if (average >= 3m && average < 3.4m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "3star.png");
                    }
                    else if (average >= 3.5m && average < 3.9m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "3.5star.png");
                    }
                    else if (average >= 4m && average < 4.4m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "4star.png");
                    }
                    else if (average >= 4.5m && average < 4.9m)
                    {
                        ViewBag.StarRating = Path.Combine(path + "4.5star.png");
                    }
                    else
                    {
                        ViewBag.StarRating = Path.Combine(path + "5star.png");
                    }

                    //Set the calculations to a ViewBag for display within the View.
                    ViewBag.Rating = displayAverage;
                    ViewBag.Comments = displayCommentCount;
                    response = View(displayGame);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If an issue occurs, redirect to the Index of Games.
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            else
            {
                response = RedirectToAction("Index", "Games");
            }

            return response;
        }

        [HttpGet]
        public ActionResult AddGame()
        {
            //Only administrators and contributors can add games.
            ActionResult response = null;
            if (Session["UserRole"] != null && (int)Session["UserRole"] < 3)
            {
                response = View();
            }
            else
            {
                response = RedirectToAction("Index", "Games");
            }
            return response;
        }

        //Take the information from the user and add a game to the database, including pictures and game files.
        [HttpPost]
        public ActionResult AddGame(HttpPostedFileBase picture, HttpPostedFileBase game, GamePO form)
        {
            //Variables for path values.
            string downloadPath = string.Empty;
            string picturePath = string.Empty;

            ActionResult response = null;
            //Only contributors and administrators can add games to the database.
            if (ModelState.IsValid && (Session["UserRole"] != null && (int)Session["UserRole"] < 3))
            {
                try
                {
                    //Send game and picture files to a method for acceptance.
                    //A bool value deterimes the type of file submitted from the form.
                    form.Download = AcceptFile(game, downloadPath, false);
                    form.Picture = AcceptFile(picture, picturePath, true);

                    //Add the game's information to the database and redirect to the games index.
                    GameDO gameObject = Mapper.GamePOtoDO(form);
                    _dataAccess.AddGame(gameObject);
                    response = RedirectToAction("Index", "Games");
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If an issue occurs while adding a game, redirect to the games index.
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            //If there was a problem with the model while adding a game, give the user back the form info.
            else
            {
                response = View(form);
            }
            return response;
        }

        //Update a game specific to the GameId.
        [HttpGet]
        public ActionResult UpdateGame(int specificGame = default(int))
        {
            // Only administrators and contributors can update games.
            ActionResult response = null;
            if ((Session["UserRole"] != null && (int)Session["UserRole"] < 3) || specificGame != 0)
            {
                try
                {
                    //Give the user the game's information to be updated.
                    GameDO gameObject = _dataAccess.GameDetails(specificGame);
                    GamePO displayGame = Mapper.GameDOtoPO(gameObject);
                    response = View(displayGame);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If there is an issue, send the user to the game's index.
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            else
            {
                response = RedirectToAction("Index", "Games");
            }
            return response;
        }

        //Update a game with the information provided by the user, including game and picture files.
        [HttpPost]
        public ActionResult UpdateGame(GamePO form, HttpPostedFileBase picture, HttpPostedFileBase game)
        {
            //Set variables for the file paths.
            string downloadPath = string.Empty;
            string picturePath = string.Empty;

            ActionResult response = null;
            //Only contributors and administrators can update the games.
            if (ModelState.IsValid && (Session["UserRole"] != null && (int)Session["UserRole"] < 3))
            {
                try
                {
                    //Send game and picture files to a method for acceptance.
                    //A bool value deterimes the type of file submitted from the form.
                    downloadPath = form.Download;
                    picturePath = form.Picture;

                    //Apply the selected files to the form if they were accepted.
                    form.Download = AcceptFile(game, downloadPath, false);
                    form.Picture = AcceptFile(picture, picturePath, true);

                    //Send the form to the database to be applied to the selected game.
                    GameDO gameObject = Mapper.GamePOtoDO(form);
                    _dataAccess.UpdateGame(gameObject);

                    //Run the remove unused file method to maintian efficient storage usage.
                    RemovePriorFile();
                    response = RedirectToAction("GameDetails", "Games", new { specificGame = form.GameID });
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If can issue occurs, redirect to the Index page.
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            else
            {
                //If there was an issue with the model, give the user their input information back.
                response = View(form);
            }
            return response;
        }

        //Removes a game from the database at a specific GameId.
        [HttpGet]
        public ActionResult RemoveGame(int specificGame)
        {
            //Only administrators can remove a game.
            ActionResult response = null;
            if ((int)Session["UserRole"] == 1)
            {
                try
                {
                    _dataAccess.RemoveGame(specificGame);
                    response = RedirectToAction("Index", "Games");
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If an issue occurs, redirect the user to the Index of Games page.
                    response = RedirectToAction("Index", "Games");
                }
                finally
                { }
            }
            else
            {
                //If a user somehow accesses the Remove function of the site, redirect them to specific game's page.
                response = RedirectToAction("GameDetails", "Games", new { specificGame = RemoveGame(specificGame) });
            }
            return response;
        }

        //Allows the user to download files of games from the website.
        public ActionResult DownloadGame(string gameName)
        {
            //Sets the path to the game file with the game's filename.
            string gameFilePath = Server.MapPath(gameName);
            //Opens a stream to allow access to the selected file.
            FileStream stream = new FileStream(gameFilePath, FileMode.Open, FileAccess.Read);
            //Return the file as located by the stream, using Octet to allow the filetype to be given.
            return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(gameName));
        }

        //Method that takes the files uploaded from the user and saves them to server.
        private string AcceptFile(HttpPostedFileBase file, string filePath, bool isPicture)
        {
            //If a new file is provided from the user.
            if (file != null && file.ContentLength > 0)
            {
                //Send the file to be processed with a unique name.
                string newName = GenerateUniqueFileName(file);
                //Save the file to the server once a unique name has been applied.
                file.SaveAs(newName);
                //Change the filepath of the file to be recognized by the database as an acceptable path.
                //Adds a slash to the beginning and replaces all doouble backslashes with forward slashes.
                newName = "/" + newName.Replace(Server.MapPath("/"), "").Replace("\\", "/");
                //Remove characters from the path that are not needed and can cause errors.
                filePath = newName.Replace(Server.MapPath("~/"), "");
            }
            //If a file path existed in the form already and the user didn't upload a new one, use the current.
            else if (!string.IsNullOrWhiteSpace(filePath))
            {
                //do nothing
            }
            //If there wasn't a file provided, nor already present:
            else
            {
                //If this method was called to check the form for a picture, set the picture to default.
                if (isPicture)
                {
                    filePath = Constants.DefaultImagePath;
                }
                //Sets the form to null when the method is called for checking a form for a game file.
                else
                {
                    filePath = null;
                }
            }
            return filePath;
        }
        
        //Method that applies a unique name to a file when one is provided by the user.
        private string GenerateUniqueFileName(HttpPostedFileBase submittedFile)
        {
            //Sets a variable to apply completed file path to.
            string responsePath = string.Empty;

            //Ensure the data is valid and not empty.
            if (submittedFile.ContentLength > 0)
            {
                //Gets the full path to the folder where files are stored.
                //Grabs the extension from the file that was submitted.
                //Grabs the name of the file without the extension.
                string fullPathToFolder = Server.MapPath(Constants.GameFilesPath);
                string fileExtension = Path.GetExtension(submittedFile.FileName);
                string fileWithoutExtension = Path.GetFileNameWithoutExtension(submittedFile.FileName);

                //Entire path of the file.
                responsePath = fullPathToFolder + fileWithoutExtension + fileExtension;
                
                //If a file already exists at the location of the given path:
                if (System.IO.File.Exists(responsePath))
                {
                    //Add the collision to the file submitted.
                    int collisions = 1;
                    FileInfo fileDetails = new FileInfo(responsePath);

                    responsePath = Path.Combine(Server.MapPath(Constants.GameFilesPath), fileWithoutExtension + "(1)" + fileDetails.Extension);

                    //Generate a new file path as well, adding collision until a filename is not in use.
                    while (System.IO.File.Exists(responsePath))
                    {
                        collisions++;
                        responsePath = responsePath.Replace(string.Format("({0})", collisions - 1), string.Format("({0})", collisions));
                    }
                }
            }
            //Return the filepath with filename as accepted by the method.
            return responsePath;
        }

        //Removes any files within the game files directory that are not currently in use.
        private void RemovePriorFile()
        {
            //The path to the game files folder.
            string path = Server.MapPath(Constants.GameFilesPath);

            //Get a list of all files within that folder.
            List<string> filesInFolder = Directory.GetFiles(path).ToList();
            //Get a list of all files within the database.
            List<GameDO> dbFiles = _dataAccess.ViewGames();

            //Loop will check each file in the game files folder against the paths recorded in the database.
            foreach (string fileLocation in filesInFolder)
            {
                //booleon defaulted to no file found for every file in the folder.
                bool fileFound = false;
                foreach (GameDO game in dbFiles)
                {
                    //if the filepath in the folder and the filepath in the database match up, the booleon is true.
                    if (Server.MapPath(game.Picture).Equals(fileLocation) || 
                        Server.MapPath(game.Download).Equals(fileLocation))
                    {
                        //file was found
                        fileFound = true;
                    }
                }
                //if a file in the folder was found but didnt have a matching path in the database, delete it.
                //Unless the file is the default image file, which may or may not be in use, don't delete that.
                if (!fileFound && fileLocation != Server.MapPath(Constants.DefaultImagePath))
                {
                    System.IO.File.Delete(fileLocation);
                }
            }
        }
    }
}