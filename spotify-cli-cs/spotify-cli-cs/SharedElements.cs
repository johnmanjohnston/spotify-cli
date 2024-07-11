using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using spotify_cli_cs.Utility;

namespace spotify_cli_cs
{
    static class SharedElements
    {
        public static EdgeDriver? driver;

        public static IWebElement GetPlayPauseButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-playpause']"));
        public static IWebElement GetSongNameLink() => driver!.FindElement(By.XPath("//a[@data-testid='context-item-link']"));
        public static IWebElement GetArtistNameLink() => driver!.FindElement(By.XPath("//a[@data-testid='context-item-info-artist']"));
        public static IWebElement GetSkipBackButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-skip-back']"));
        public static IWebElement GetSkipForwardButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-skip-forward']"));
        public static IWebElement GetHeartButton() => driver!.FindElement(By.XPath("//button[@data-testid='add-button']"));
        public static IWebElement? GetSaveButtonForCurrentSong() // TODO: refactor this function
        {
            // im not sure if this is working exactly as it is supposed to, but i'm too lazy to fix it. if it works for what it's needed for, don't touch it.
            try 
            { 
                return driver!.FindElement(By.XPath("//button[@data-encore-id='buttonTertiary'][@aria-label='Add to playlist'][@class='Button-sc-1dqy6lx-0 fQMoMb']"));
            }

            catch 
            {
                try
                {
                    StaticUtilities.DBG("Finding the first save button failed, going for the secocnd one");
                    return driver!.FindElement(By.XPath("//button[@data-encore-id='buttonTertiary'][@aria-label='Add to playlist'][@class'Button-sc-1dqy6lx-0 fLalFV']"));
                }

                catch { return null; }
            }

        }
        public static IWebElement GetProgressBarDiv() => driver!.FindElement(By.XPath("//div[@data-testid='progress-bar']"));
        public static IWebElement GetRepeatButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-repeat']"));
        public static IWebElement GetShuffleButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-shuffle']"));
        public static IWebElement GetPlaybackPositionDiv() => driver!.FindElement(By.XPath("//div[@data-testid='playback-position']"));
        public static IWebElement GetPlaybackDurationDiv() => driver!.FindElement(By.XPath("//div[@data-testid='playback-duration']"));
        public static IWebElement GetNextSongLink() => driver!.FindElements(By.XPath("//a[@data-testid='context-item-link']"))[2];
        public static IWebElement GetMuteButton() => driver!.FindElement(By.XPath("//button[@data-testid='volume-bar-toggle-mute-button']"));

        // public static IWebElement GetNextSongArtistLink() => driver!.FindElements(By.XPath("//a[@data-testid='context-item-info-artist']"))[2];
        
        // context-item-info-subtitles then get first element with context-item-info-artist
        public static IWebElement GetNextSongArtistLink()
        {
            var subs = driver!.FindElements(By.XPath("//div[@data-testid='context-item-info-subtitles']"))[2];
            var el = subs.FindElements(By.XPath(".//a[@data-testid='context-item-info-artist']"))[0];
            return el;
        }

        public static IWebElement GetNowPlayingViewButton() => driver!.FindElement(By.XPath("//button[@data-testid='control-button-npv']"));

        public static IWebElement GetPlaylistTrackList() => driver!.FindElement(By.XPath("//div[@data-testid='playlist-tracklist']"));
        public static IWebElement GetTracklist() => driver!.FindElement(By.XPath("//div[@data-testid='track-list']"));
        public static IWebElement CurrentContextTracklist() => GetPlaylistTrackList() ?? GetTracklist();

        public static List<IWebElement> CurrentTracklistSongChunk() 
        {
            // TO DO: REMOVE THE NEDDLESS CONVERSION OF LIST -> ARRAY -> LIST

            int maxElementsInChunk = 10;
            return CurrentContextTracklist()
            .FindElements(By.XPath(".//div[@data-testid='tracklist-row']"))
            .Take(maxElementsInChunk)
            .ToList();
        }
    }
}