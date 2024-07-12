using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

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

        public static void ToggleMute()
        {
            SharedElements.GetMuteButton().Click();
        }

        public static void ChangeRepeatMode()
        {
            SharedElements.GetRepeatButton().Click();
        }

        public static void ChangeShuffleMode()
        {
            SharedElements.GetShuffleButton().Click();
        }

        public static void GoToItemWithUri(string uri, EdgeDriver driver)
        {
            // get search btn
            var searchBtn = driver.FindElement(By.XPath("//a[@href='/search']"));
            searchBtn.Click();

            Thread.Sleep(500);

            var searchInput = SpotifyCLI.driver.FindElement(By.XPath("//input[@data-testid='search-input']"));
            searchInput.Click();
            searchInput.SendKeys(uri);
            searchInput.Submit();
        }
    }   
}