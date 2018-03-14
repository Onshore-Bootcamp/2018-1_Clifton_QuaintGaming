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
    public class UsersController : Controller
    {
        //Contructor that sets the connection string to variables for use throughout the controller.
        public UsersController()
        {
            string connection = ConfigurationManager.ConnectionStrings["dataSource"].ConnectionString;
            _dataAccess = new UserDAO(connection);
        }

        //Connection variable.
        public UserDAO _dataAccess;

        //Get a list of all users in the database.
        [HttpGet]
        public ActionResult Index()
        {
            ActionResult response = null;
            //User table is only accessible to Administrators
            if (Session["UserRole"] != null && (int)Session["UserRole"] == 1)
            {
                try
                {
                    // Get list of all users from database and map to display list in the view
                    List<UserDO> userList = _dataAccess.ViewUsers();
                    List<UserPO> displayUsers = Mapper.ListUserDOtoPO(userList);
                    response = View(displayUsers);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If there was an issue retrieving the list of users, redirect to the Index of Games.
                    response = RedirectToAction("Index", "Games");
                }
                finally
                { }
            }
            else
            {
                response = RedirectToAction("Index", "Games");
            }
            return response;
        }

        [HttpGet]
        public ActionResult RegisterUser()
        {
            // Only unregistered users and administrators.
            ActionResult response = null;
            if (Session["UserID"] == null || (int)Session["UserRole"] == 1)
            {
                UserPO model = new UserPO();
                //If accessed by an administrator, show add instead of Register via TempData.
                if (Session["UserRole"] != null && (int)Session["UserRole"] == 1)
                {
                    TempData["AddOrReg"] = "Add New User";
                }
                response = View(model);
            }
            else
            {
                response = RedirectToAction("Index", "Games");
            }
            return response;
        }

        //Add a user to the database
        [HttpPost]
        public ActionResult RegisterUser(UserPO form)
        {
            ActionResult response = null;
            //Only unregistered users and administrators can submit registration forms.
            if (ModelState.IsValid && (Session["UserID"] == null || (int)Session["UserRole"] == 1))
            {
                try
                {
                    //Check whether the username already exists, and let the user know if so.
                    UserDO user = _dataAccess.UserLogin(form.Username);
                    if (user.Username.Equals(form.Username))
                    {
                        ModelState.AddModelError(form.Username, "This username is already in use.");
                        response = View(form);
                    }
                    else
                    {
                        //Accept the registration and set session if the username chosen is valid.
                        UserDO userObject = Mapper.UserPOtoDO(form);
                        _dataAccess.RegisterUser(userObject);
                        if (user != null)
                        {
                            SetSession(user);
                            response = RedirectToAction("Index", "Games");
                        }
                        else
                        {
                            //Return them to the form with the input information if not valid.
                            response = View(form);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //If an issue occurs, send the user to the Index of Games page.
                    Logger.Log(ex);
                    response = RedirectToAction("Index", "Games");
                }
                finally
                { }
            }
            else
            {
                //If there's an issue with model state, give the user back the information on the form.
                response = View(form);
            }
            return response;
        }

        //Get the user's login information.
        [HttpGet]
        public ActionResult UserLogin()
        {
            //Login page only accessible to an unsigned in user.
            ActionResult response = null;
            if (Session["UserID"] == null)
            {
                response = View();
            }
            return response;
        }

        //Check the user's credentials against the database and accept if correct.
        [HttpPost]
        public ActionResult UserLogin(LoginPO form)
        {
            ActionResult response = null;
            //Only login users who are not already logged in.
            if (ModelState.IsValid && Session["UserID"] == null)
            {
                try
                {
                    //Check the database for the input username and make sure the input password matches.
                    UserDO user = _dataAccess.UserLogin(form.Username);
                    if (user != null && form.Password.Equals(user.Password))
                    {
                        //When username and password match, set session to the role the user has assigned.
                        SetSession(user);
                        response = RedirectToAction("Index", "Games");
                    }
                    else
                    {
                        //Give back the form and an error if they do not match.
                        ModelState.AddModelError(form.Password, "Username or Password was incorrect");
                        response = View(form);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If a problem occurs, redirect the user to the login page.
                    response = RedirectToAction("UserLogin", "Users");
                }
                finally { }
            }
            else
            {
                //If there's an issue with submitting the form, redirect the user to the Index of Games page.
                response = RedirectToAction("Index", "Games");
            }
            return response;
        }

        //The user logging out abandons session and redirects the user.
        [HttpGet]
        public ActionResult UserLogout()
        {
            if (Session["UserID"] != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Index", "Games");
        }

        //Display the user's information.
        [HttpGet]
        public ActionResult UserDetails(int specificUser = default(int))
        {
            ActionResult response = null;
            //Only accessiblle to signed in users.
            if (Session["UserRole"] != null)
            {
                //The details page must match the user Id of the user requesting, or be an admin.
                if (specificUser != (int)Session["UserID"] && (int)Session["UserRole"] != 1)
                {
                    //Make them match if they don't.
                    specificUser = (int)Session["UserID"];
                }
                try
                {
                    //Get and display the users information.
                    UserDO userObject = _dataAccess.UserDetails(specificUser);
                    UserPO displayUser = Mapper.UserDOtoPO(userObject);
                    response = View(displayUser);

                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If there is an issue getting the user's details, sent the user to the Index of Games.
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            else
            {
                //Redirect if the user does not have session.
                response = RedirectToAction("Index", "Games");
            }
            return response;
        }

        [HttpGet]
        public ActionResult UpdateUser(int specificUser = default(int))
        {
            ActionResult response = null;
            //Only registered users can update their information.
            if (Session["UserRole"] != null)
            {
                //The user accessing the update page must be updating their own information, unless they are admin.
                if (specificUser != (int)Session["UserID"] && (int)Session["UserRole"] != 1)
                {
                    //Make them match if they don't.
                    specificUser = (int)Session["UserID"];
                }
                try
                {
                    //Populate the form with the user's current information.
                    UserDO userObject = _dataAccess.UserDetails(specificUser);
                    UserPO displayUser = Mapper.UserDOtoPO(userObject);
                    response = View(displayUser);
                }
                catch (Exception ex)
                {
                    //If there is an issue, send the user to the game's index.
                    Logger.Log(ex);
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            else
            {
                //If the user has lost session, redirect them.
                response = RedirectToAction("Index", "Games");
            }
            return response;
        }

        //Apply the input information to the user in the database.
        [HttpPost]
        public ActionResult UpdateUser(UserPO form)
        {
            ActionResult response = null;
            //User must be signed in.
            if (ModelState.IsValid && Session["UserID"] != null)
            {
                try
                {
                    //Send the information from the form and apply it to the user's info in the database.
                    UserDO userObject = Mapper.UserPOtoDO(form);
                    _dataAccess.UpdateUser(userObject);

                    //Send admins to the index of users after update.
                    if ((int)Session["UserRole"] == 1)
                    {
                        response = RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        //Send non admin users to their user details page.
                        response = RedirectToAction("UserDetails", "Users", new { specificUser = form.UserID });
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If an issue occurs, send the user to the Index of Games.
                    response = RedirectToAction("Index", "Games");
                }
                finally { }
            }
            else
            {
                //If the model is not accepted, attempt to send them to their details page.
                response = RedirectToAction("UserDetails", "Users", new { specificUser = form.UserID });
            }
            return response;
        }


        [HttpGet]
        public ActionResult RemoveUser(int specificUser = default(int), int userRole = default(int))
        {
            //Only administrators can delete users other than themselves
            ActionResult response = null;
            if ((Session["UserID"] != null && (specificUser == (int)Session["UserID"]) || (int)Session["UserRole"] == 1))
            {
                try
                {
                    //Administrators cannot be deleted
                    if (userRole != 1)
                    {
                        _dataAccess.RemoveUser(specificUser);
                        if ((int)Session["UserRole"] == 1)
                        {
                            //Redirect admin to the users index.
                            response = RedirectToAction("Index", "Users");
                        }
                        else
                        {
                            //Once a user closes their account, log them out and send them to the game's index.
                            UserLogout();
                            response = RedirectToAction("Index", "Games");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(ex);
                    //If an issue occurs, send the user to the Index of Games.
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

        //Set session upon login, storing their username, userId and their given role.
        private void SetSession(UserDO user)
        {
            Session["Username"] = user.Username;
            Session["UserID"] = user.UserID;
            Session["UserRole"] = user.RoleID;
            Session.Timeout = 10;
        }
    }
}