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
    }   
}