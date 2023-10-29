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
        }

        public static void SkipBack()
        {
            SharedElements.GetSkipBackButton().Click();
        }

        public static void ToggleHeart()
        {
            SharedElements.GetHeartButton().Click();
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