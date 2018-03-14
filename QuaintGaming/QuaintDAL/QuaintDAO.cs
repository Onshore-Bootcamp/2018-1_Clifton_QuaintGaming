using System;
using System.Collections.Generic;
using QuaintDAL.Logging;
using QuaintDAL.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace QuaintDAL
{
    public class QuaintDAO
    {
        private readonly string _connectionString;

        //Contructor sets the connection string value to be used throughout the class.
        public QuaintDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<GameDO> ViewGames()
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;
            SqlDataReader reader = null;
            List<GameDO> gameList = new List<GameDO>();
            GameDO gameObject = new GameDO();

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("OBTAIN_GAMES", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Tell SQLDataReader to gather the information from SQL as determined by the stored procedure.
                reader = storedProcedure.ExecuteReader();

                //Cycle through the database and apply each property in SQL to an object
                //Then add that object to a list of objects.
                while (reader.Read())
                {
                    gameObject = new GameDO();
                    gameObject.GameID = int.Parse(reader["GameID"].ToString());
                    gameObject.GameName = reader["GameName"].ToString();
                    gameObject.ReleaseYear = short.Parse(reader["ReleaseYear"].ToString());
                    gameObject.Genre = reader["Genre"].ToString();
                    gameObject.Developer = reader["Developer"].ToString();
                    gameObject.Publisher = reader["Publisher"].ToString();
                    gameObject.Platform = reader["Platform"].ToString();
                    gameObject.Download = reader["Download"].ToString();
                    gameObject.Picture = reader["Picture"].ToString();
                    gameObject.Description = reader["Description"].ToString();
                    gameList.Add(gameObject);
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
            return gameList;
        }
        
        public GameDO GameDetails(int GameID)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;
            SqlDataReader reader = null;
            GameDO gameObject = new GameDO();

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("VIEW_GAME", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Applies the game Id given to the method to the stored procedure.
                storedProcedure.Parameters.AddWithValue("@GameID", GameID);

                //Opens the connection to SQL.
                connectionToSql.Open();
                //Tell SQLDataReader to gather the information from SQL as determined by the stored procedure.
                reader = storedProcedure.ExecuteReader();

                //Reads all the information in SQL via the stored procedure and applies the data to the object.
                while (reader.Read())
                {
                    gameObject.GameID = int.Parse(reader["GameID"].ToString());
                    gameObject.GameName = reader["GameName"].ToString();
                    gameObject.ReleaseYear = short.Parse(reader["ReleaseYear"].ToString());
                    gameObject.Genre = reader["Genre"].ToString();
                    gameObject.Developer = reader["Developer"].ToString();
                    gameObject.Publisher = reader["Publisher"].ToString();
                    gameObject.Platform = reader["Platform"].ToString();
                    gameObject.Download = reader["Download"].ToString();
                    gameObject.Picture = reader["Picture"].ToString();
                    gameObject.Description = reader["Description"].ToString();
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
            return gameObject;
        }

        public void AddGame(GameDO form)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Setup for connecting to SQl and accessing the stored procedure.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("ADD_GAME", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the value from the object to the parameters in the stored procedure to SqlCommand.
                storedProcedure.Parameters.AddWithValue("@GameName", form.GameName);
                storedProcedure.Parameters.AddWithValue("@ReleaseYear", form.ReleaseYear);
                storedProcedure.Parameters.AddWithValue("@Genre", form.Genre);
                storedProcedure.Parameters.AddWithValue("@Developer", form.Developer);
                storedProcedure.Parameters.AddWithValue("@Publisher", form.Publisher);
                storedProcedure.Parameters.AddWithValue("@Platform", form.Platform);
                storedProcedure.Parameters.AddWithValue("@Download", form.Download);
                storedProcedure.Parameters.AddWithValue("@Picture", form.Picture);
                storedProcedure.Parameters.AddWithValue("@Description", form.Description);

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

        public void UpdateGame(GameDO form)
        {
            //Setting the variables to be used for this method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("UPDATE_GAME", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;

                //Add the value from the object to the parameters in the stored procedure to SqlCommand.
                storedProcedure.Parameters.AddWithValue("@GameID", form.GameID);
                storedProcedure.Parameters.AddWithValue("@GameName", form.GameName);
                storedProcedure.Parameters.AddWithValue("@ReleaseYear", form.ReleaseYear);
                storedProcedure.Parameters.AddWithValue("@Genre", form.Genre);
                storedProcedure.Parameters.AddWithValue("@Developer", form.Developer);
                storedProcedure.Parameters.AddWithValue("@Publisher", form.Publisher);
                storedProcedure.Parameters.AddWithValue("@Platform", form.Platform);
                storedProcedure.Parameters.AddWithValue("@Download", form.Download);
                storedProcedure.Parameters.AddWithValue("@Picture", form.Picture);
                storedProcedure.Parameters.AddWithValue("@Description", form.Description);

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
                connectionToSql.Close();
                connectionToSql.Dispose();
            }
        }

        public void RemoveGame(int GameID)
        {
            //Setting the variabes that will be used within the method.
            SqlConnection connectionToSql = null;
            SqlCommand storedProcedure = null;

            try
            {
                //Set up for connecting to SQL, using the stored procedure and how to access the information.
                connectionToSql = new SqlConnection(_connectionString);
                storedProcedure = new SqlCommand("REMOVE_GAME", connectionToSql);
                storedProcedure.CommandType = System.Data.CommandType.StoredProcedure;
                //Gives the value passed to the method to the stored procedure parameter.
                storedProcedure.Parameters.AddWithValue("@GameID", GameID);

                //Open the connection to SQL.
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
    }
}
