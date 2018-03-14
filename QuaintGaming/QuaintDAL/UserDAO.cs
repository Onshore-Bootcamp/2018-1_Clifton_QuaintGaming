using QuaintDAL.Logging;
using QuaintDAL.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaintDAL
{
    public class UserDAO
    {
        private readonly string _connectionString;

        //Contructor sets the connection string value to be used throughout the class.
        public UserDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<UserDO> ViewUsers()
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;
            SqlDataReader reader = null;
            List<UserDO> userList = new List<UserDO>();
            UserDO userObject = new UserDO();

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("OBTAIN_USERS", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Tell SQLDataReader to gather the information from SQL as determined by the stored procedure.
                reader = storedProcedure.ExecuteReader();

                //Cycle through the database and apply each property in SQL to an object
                //Then add that object to a list of objects.
                while (reader.Read())
                {
                    userObject = new UserDO();
                    userObject.UserID = int.Parse(reader["UserID"].ToString());
                    userObject.Username = reader["Username"].ToString();
                    userObject.FirstName = reader["FirstName"].ToString();
                    userObject.LastName = reader["LastName"].ToString();
                    userObject.Email = reader["Email"].ToString();
                    userObject.RoleID = int.Parse(reader["RoleID"].ToString());
                    userObject.Title = reader["Title"].ToString();
                    userList.Add(userObject);
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw;
            }
            finally
            {
                //When the connection has been established, close and dispose the connection before finishing.
                if (connectionToSql != null)
                {
                    connectionToSql.Close();
                    connectionToSql.Dispose();
                }
            }
            return userList;
        }

        public UserDO UserDetails(int UserID)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;
            SqlDataReader reader = null;
            UserDO userObject = new UserDO();

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("VIEW_USER", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Applies the game Id given to the method to the stored procedure.
                storedProcedure.Parameters.AddWithValue("@UserID", UserID);

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Tell SQLDataReader to gather the information from SQL as determined by the stored procedure.
                reader = storedProcedure.ExecuteReader();

                //Reads all the information in SQL via the stored procedure and applies the data to the object.
                while (reader.Read())
                {
                    userObject.UserID = int.Parse(reader["UserID"].ToString());
                    userObject.Username = reader["Username"].ToString();
                    userObject.Password = reader["Password"].ToString();
                    userObject.FirstName = reader["FirstName"].ToString();
                    userObject.LastName = reader["LastName"].ToString();
                    userObject.Email = reader["Email"].ToString();
                    userObject.RoleID = int.Parse(reader["RoleID"].ToString());
                    userObject.Title = reader["Title"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw;
            }
            finally
            {
                //When the connection has been established, close and dispose the connection before finishing.
                if (connectionToSql != null)
                {
                    connectionToSql.Close();
                    connectionToSql.Dispose();
                }
            }
            return userObject;
        }

        public UserDO UserLogin(string Username)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;
            SqlDataReader reader = null;
            UserDO userObject = new UserDO();

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("VIEW_USER_USERNAME", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Gives the value passed to the method to the stored procedure parameter.
                storedProcedure.Parameters.AddWithValue("@Username", Username);

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Tell SQLDataReader to gather the information from SQL as determined by the stored procedure.
                reader = storedProcedure.ExecuteReader();

                //Reads all the information in SQL via the stored procedure and applies the data to the object.
                while (reader.Read())
                {
                    userObject.UserID = int.Parse(reader["UserID"].ToString());
                    userObject.Username = reader["Username"].ToString();
                    userObject.Password = reader["Password"].ToString();
                    userObject.FirstName = reader["FirstName"].ToString();
                    userObject.LastName = reader["LastName"].ToString();
                    userObject.Email = reader["Email"].ToString();
                    userObject.RoleID = int.Parse(reader["RoleID"].ToString());
                    userObject.Title = reader["Title"].ToString();
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw;
            }
            finally
            {
                //When the connection has been established, close and dispose the connection before finishing.
                if (connectionToSql != null)
                {
                    connectionToSql.Close();
                    connectionToSql.Dispose();
                }
            }
            return userObject;
        }

        public void RegisterUser(UserDO form)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Setup for connecting to SQl and accessing the stored procedure.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("ADD_USER", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the value from the object to the parameters in the stored procedure to SqlCommand.
                storedProcedure.Parameters.AddWithValue("@Username", form.Username);
                storedProcedure.Parameters.AddWithValue("@Password", form.Password);
                storedProcedure.Parameters.AddWithValue("@FirstName", form.FirstName);
                storedProcedure.Parameters.AddWithValue("@LastName", form.LastName);
                storedProcedure.Parameters.AddWithValue("@Email", form.Email);
                storedProcedure.Parameters.AddWithValue("@RoleID", form.RoleID);

                //Open connection to SQL.
                connectionToSql.Open();
                //Applies all the values stored in the SqlCommand to the database.
                storedProcedure.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw;
            }
            finally
            {
                //When the connection has been established, close and dispose the connection before finishing.
                if (connectionToSql != null)
                {
                    connectionToSql.Close();
                    connectionToSql.Dispose();
                }
            }
        }

        public void UpdateUser(UserDO form)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("UPDATE_USER", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the value from the object to the parameters in the stored procedure to SqlCommand.
                storedProcedure.Parameters.AddWithValue("@UserID", form.UserID);
                storedProcedure.Parameters.AddWithValue("@Username", form.Username);
                storedProcedure.Parameters.AddWithValue("@Password", form.Password);
                storedProcedure.Parameters.AddWithValue("@FirstName", form.FirstName);
                storedProcedure.Parameters.AddWithValue("@LastName", form.LastName);
                storedProcedure.Parameters.AddWithValue("Email", form.Email);
                storedProcedure.Parameters.AddWithValue("@RoleID", form.RoleID);

                //Open the connection to SQL.
                connectionToSql.Open();
                //Applies all the values stored in the SqlCommand to the database.
                storedProcedure.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw;
            }
            finally
            {
                //When the connection has been established, close and dispose the connection before finishing.
                if (connectionToSql != null)
                {
                    connectionToSql.Close();
                    connectionToSql.Dispose();
                }
            }
        }
        
        public void RemoveUser(int UserID)
        {
            //Setting the variabes that will be used within the method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("REMOVE_USER", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Gives the value passed to the method to the stored procedure parameter.
                storedProcedure.Parameters.AddWithValue("UserID", UserID);

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Applies the stored procedure in the SqlCommand to the database.
                storedProcedure.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw;
            }
            finally
            {
                //When the connection has been established, close and dispose the connection before finishing.
                if (connectionToSql != null)
                {
                    connectionToSql.Close();
                    connectionToSql.Dispose();
                }
            }
        }
    }
}
