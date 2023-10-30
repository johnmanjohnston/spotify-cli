using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace spotify_cli_cs
{
    static class SharedElements
    {
        public static ChromeDriver? driver;

        public static IWebElement GetPlayPauseButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-playpause']"));
        public static IWebElement GetSongNameLink() => driver!.FindElement(By.XPath("//a[@data-testid='context-item-link']"));
        public static IWebElement GetArtistNameLink() => driver!.FindElement(By.XPath("//a[@data-testid='context-item-info-artist']"));
        public static IWebElement GetSkipBackButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-skip-back']"));
        public static IWebElement GetSkipForwardButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-skip-forward']"));
        public static IWebElement GetHeartButton() => driver!.FindElement(By.XPath("//button[@data-testid='add-button']"));
        public static IWebElement GetProgressBarDiv() => driver!.FindElement(By.XPath("//div[@data-testid='progress-bar']"));
        public static IWebElement GetRepeatButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-repeat']"));
        public static IWebElement GetShuffleButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-shuffle']"));
        public static IWebElement GetPlaybackPositionDiv() => driver!.FindElement(By.XPath("//div[@data-testid='playback-position']"));
        public static IWebElement GetPlaybackDurationDiv() => driver!.FindElement(By.XPath("//div[@data-testid='playback-duration']"));
    }
}