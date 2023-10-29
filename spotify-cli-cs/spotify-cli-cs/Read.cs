using System;

namespace spotify_cli_cs 
{
    static class Read
    {
        public static string GetCurrentlyPlaying()
        {
            char HEARTED = '♥';
            char UNHEARTED = '♡';

            try {
                bool isHearted = SharedElements.GetHeartButton().GetAttribute("aria-checked").ToLower() == "true";
                char resultingHeartedChar = isHearted ? HEARTED : UNHEARTED;

                string retval = $"{SharedElements.GetSongNameLink().Text} - {SharedElements.GetArtistNameLink().Text} - {resultingHeartedChar}";
                return retval;
            } catch (Exception)
            {
                return "Loading...";
            }
        }

        public static string GetPlaybackDetails()
        {
            return "TODO";
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