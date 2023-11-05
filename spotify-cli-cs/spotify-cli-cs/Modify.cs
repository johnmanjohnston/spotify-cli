using OpenQA.Selenium;

namespace spotify_cli_cs
{
    static class Modify
    {
        private static void l(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }

        public static void TogglePlayPause()
        {
            SharedElements.GetPlayPauseButton().Click();
        }

        public static void SkipForward()
        {
            l("skipping frwrd");
            SharedElements.GetSkipForwardButton().Click();
            // SharedElements.GetNowPlayingViewButton().Click();
            // SharedElements.GetNowPlayingViewButton().Click();
        }

        public static void SkipBack()
        {
            SharedElements.GetSkipBackButton().Click();
            // SharedElements.GetNowPlayingViewButton().Click();
            // SharedElements.GetNowPlayingViewButton().Click();
        }

        public static void ToggleHeart()
        {
            try
            {
                SharedElements.GetHeartButton().Click();
            }
            catch (StaleElementReferenceException) { }
        }

        public static void ChangeRepeatMode()
        {
            SharedElements.GetRepeatButton().Click();
        }

        public static void ChangeShuffleMode()
        {
            SharedElements.GetShuffleButton().Click();
        }
    }   
}