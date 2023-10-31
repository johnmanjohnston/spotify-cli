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

        public static char GetHeartedStatus() 
        {
            if (SpotifyCLI.FRONTEND_ONLY) return '!';

            char HEARTED = '♥';
            char UNHEARTED = '♡';
         
            try
            {
                bool isHearted = SharedElements.GetHeartButton().GetAttribute("aria-checked").ToLower() == "true";

                return isHearted ? HEARTED : UNHEARTED;
            }

            catch
            {
                return '-';
            }
        }

        public static string GetPlaybackTimeInfo()
        {
            if (SpotifyCLI.FRONTEND_ONLY) return "read.getPlaybackTimeInfo() called with frontend only enabled";

            try
            {
                string pos = SharedElements.GetPlaybackPositionDiv().Text;
                string duration = SharedElements.GetPlaybackDurationDiv().Text.TrimStart('-');

                string retval = $"{pos} / {duration}";

                return retval;
            }

            catch
            {
                return "Loading...";
            }
        }

        public static string GetCurrentlyPlaying()
        {
            if (SpotifyCLI.FRONTEND_ONLY) return "Read.GetCurrentlyPlaying() and frontend only is enabeld";

            try {
                string retval = $"{SharedElements.GetSongNameLink().Text} - {SharedElements.GetArtistNameLink().Text}";
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
            if (SpotifyCLI.FRONTEND_ONLY) return "Read.GetPlaybackDetails() with frontend only is enabled";

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
            if (SpotifyCLI.FRONTEND_ONLY) return .5f;

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