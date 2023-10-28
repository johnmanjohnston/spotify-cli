using System;

namespace spotify_cli_cs 
{
    static class Read
    {
        public static string GetCurrentlyPlaying()
        {
            try {
                string retval = $"{SharedElements.GetSongNameLink().Text} - {SharedElements.GetArtistNameLink().Text}";
                return retval;
            } catch (Exception)
            {
                return "Loading...";
            }
        }
        
        public static float GetNormalizedSongProgress()
        {
            try
            {
                string progressBarStyle = SharedElements.GetProgressBarDiv().GetAttribute("style");
                var progress = Convert.ToDecimal(progressBarStyle.Split(": ")[1].Split("%")[0]);
                float retval = (float)progress / 100.0f;
                return retval;
            } catch (Exception) { return 0.0f; }
        }
    }
}