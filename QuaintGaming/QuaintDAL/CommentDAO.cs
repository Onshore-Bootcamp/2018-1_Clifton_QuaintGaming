using QuaintDAL.Logging;
using QuaintDAL.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace QuaintDAL
{
    public class CommentDAO
    {
        private readonly string _connectionString;

        //Contructor sets the connection string value to be used throughout the class.
        public CommentDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<CommentDO> ViewGameComments(int GameID)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;
            SqlDataReader reader = null;
            List<CommentDO> commentList = new List<CommentDO>();
            CommentDO commentObject = new CommentDO();

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("OBTAIN_COMMENTS", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Applies the game Id given to the method to the stored procedure.
                storedProcedure.Parameters.AddWithValue("@GameID", GameID);

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Tell SQLDataReader to gather the information from SQL as determined by the stored procedure.
                reader = storedProcedure.ExecuteReader();

                //Cycle through the database and apply each property in SQL to an object
                //Then add that object to a list of objects.
                while (reader.Read())
                {
                    commentObject = new CommentDO();
                    commentObject.CommentID = int.Parse(reader["CommentID"].ToString());
                    commentObject.CommentTime = DateTime.Parse(reader["CommentTime"].ToString());
                    commentObject.CommentText = reader["CommentText"].ToString();
                    commentObject.GameID = int.Parse(reader["GameID"].ToString());
                    commentObject.UserID = int.Parse(reader["UserID"].ToString());
                    commentObject.Username = reader["Username"].ToString();
                    commentObject.GameID = int.Parse(reader["GameID"].ToString());
                    commentList.Add(commentObject);
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw ex;
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
            return commentList;
        }

        public void AddComment(CommentDO form)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Setup for connecting to SQl and accessing the stored procedure.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("ADD_COMMENT", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the value from the object to the parameters in the stored procedure to SqlCommand.
                storedProcedure.Parameters.AddWithValue("@CommentTime", form.CommentTime);
                storedProcedure.Parameters.AddWithValue("@CommentText", form.CommentText);
                //When the rating given is the default of byte, make it null in the database, otherwise keep it's value.
                storedProcedure.Parameters.AddWithValue("@Rating", form.Rating == default(byte) ? (object)DBNull.Value : form.Rating);
                storedProcedure.Parameters.AddWithValue("@GameID", form.GameID);
                storedProcedure.Parameters.AddWithValue("@UserID", form.UserID);

                //Open connection to Sql.
                connectionToSql.Open();
                //Applies all the values stored in the SqlCommand to the database.
                storedProcedure.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw ex;
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

        public void UpdateComment(CommentDO form)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("UPDATE_COMMENT", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the value from the object to the parameters in the stored procedure to SqlCommand.
                storedProcedure.Parameters.AddWithValue("@CommentID", form.CommentID);
                storedProcedure.Parameters.AddWithValue("@CommentTime", form.CommentTime);
                storedProcedure.Parameters.AddWithValue("@CommentText", form.CommentText);
                storedProcedure.Parameters.AddWithValue("@Rating", form.Rating);
                storedProcedure.Parameters.AddWithValue("@GameID", form.GameID);
                storedProcedure.Parameters.AddWithValue("@UserID", form.UserID);

                //Open the connection to SQL.
                connectionToSql.Open();
                //Applies all the values stored in the SqlCommand to the database.
                storedProcedure.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw ex;
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

        public void RemoveComment(int CommentID)
        {
            //Setting the variabes that will be used within the method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("REMOVE_COMMENT", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Gives the value passed to the method to the stored procedure parameter.
                storedProcedure.Parameters.AddWithValue("@CommentID", CommentID);

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Applies the stored procedure in the SqlCommand to the database.
                storedProcedure.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw ex;
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

        public List<byte> AllRatings(int GameID)
        {
            //Setting the variabes that will be used within the method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;
            SqlDataReader reader = null;
            List<byte> ratingList = new List<byte>();

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("OBTAIN_RATINGS", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Gives the value passed to the method to the stored procedure parameter.
                storedProcedure.Parameters.AddWithValue("@GameID", GameID);

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Tell SQLDataReader to gather the information from SQL as determined by the stored procedure.
                reader = storedProcedure.ExecuteReader();

                //Cycle through the database at a gameId and add all ratings to a list.
                while (reader.Read())
                {
                    ratingList.Add(byte.Parse(reader["Rating"].ToString()));
                }
            }
            catch (Exception ex)
            {
                LoggerDAL.Log(ex, "Fatal");
                //If an issue occurs, the result would likely be fatal, throw the exception.
                throw ex;
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
            return ratingList;
        }
    }
}

    
