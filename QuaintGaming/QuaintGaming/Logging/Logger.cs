﻿using System;
using System.Configuration;
using System.IO;

namespace QuaintGaming.Logging
{
    public class Logger
    {
        public static void Log(Exception ex)
        {
            //Set variables to be used by the logger.
            //The path of the log file as determind in the web config.
            StreamWriter writer = null;
            string LogPath = ConfigurationManager.AppSettings.Get("PresentationLog");
            string timeStamp = DateTime.Now.ToString();
            writer = new StreamWriter(LogPath, true);

            //Format for the logger to use while writing information to the log.
            try
            {
                writer.Write("[{0}]   {1}\nTarget:\n  {2}\nSource:\n  {3}\nMessage:\n  {4}\nStack Trace:\n  {5}\n" +
                             "END\n_______________\n",
                              timeStamp, "*Warning*", ex.TargetSite.ToString(), ex.Source, ex.Message, ex.StackTrace);
            }
            catch (IOException)
            {
                throw;
            }
            finally
            {
                //Once done with logging, close and dispose of the stream writer.
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
    }
}