namespace spotify_cli_cs
{
    static class Modify
    {
        public static void TogglePlayPause()
        {
            SharedElements.GetPlayPauseButton().Click();
        }

        public static void SkipForward()
        {
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