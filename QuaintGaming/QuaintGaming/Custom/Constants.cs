using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuaintGaming.Custom
{
    //Class of variables that don't change.
    public static class Constants
    {
        //Path of location to a pictures folder.
        public static string PicturesPath = "/Content/GamePictures/";

        //Path of location to the game files folder.
        public static string GameFilesPath = "/Content/GameFiles/";

        //Path to the default image for games added to the side without a picture given.
        public static string DefaultImagePath = "/Content/GameFiles/DefaultImage.jpg";
    }
}