using OpenQA.Selenium.DevTools.V116.Debugger;
using System;

namespace spotify_cli_cs 
{
    static class Read
    {
        private static void l(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }

        public static string GetCurrentlyPlaying()
        {
            char HEARTED = '♥';
            char UNHEARTED = '♡';

            try {
                bool isHearted = SharedElements.GetHeartButton().GetAttribute("aria-checked").ToLower() == "true";
                char resultingHeartedChar = isHearted ? HEARTED : UNHEARTED;

                string retval = $"{SharedElements.GetSongNameLink().Text} - {SharedElements.GetArtistNameLink().Text} - {resultingHeartedChar}";
                return retval;
            } catch (Exception e)
            {
                l("exception for getting cur playing");
                l(e.Message);
                return "Loading...";
            }
        }

        public static string GetPlaybackDetails()
        {
            try
            {
                string shuffleState = SharedElements.GetShuffleButton().GetAttribute("aria-checked").ToLower() == "true" ? "on" : "off";
                string repeatState = "";

                switch(SharedElements.GetRepeatButton().GetAttribute("aria-checked"))
                {
                    case "mixed":
                        repeatState = "song";
                        break;
                    case "true":
                        repeatState = "list";
                        break;
                    case "false":
                        repeatState = "off";
                        break;
                }

                return $"Shuffle: {shuffleState} - Repeat: {repeatState}";
            } 
            
            catch (Exception)
            {
                l("exception to getting palyback details");
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
            } catch (Exception) {
                l("exception for getting normalized song progress");
                return 0.0f; 
            }
        }
    }
}