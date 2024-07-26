using OpenQA.Selenium;
using spotify_cli_cs.Components.Core;
using spotify_cli_cs.Models;
using spotify_cli_cs.Utility;

namespace spotify_cli_cs.Components
{
    public class TracklistListView : BaseScrollView
    {
       // public List<string>? trackNames = new(); // assign names of track to display to the user--when a song is selected, look at the index of the selected track and find the corresponding song using selenium
        private int entriesToDisplay = 10;

        public List<TracklistItem> tracklistData = new();

        public TracklistListView(int x = 0, int y = 0) : base(x, y) { }

        public override void OnBlur()
        {
        }

        public override void OnFocus()
        {
        }

        public override void HandleKeyInput(ConsoleKey key)
        {
            base.HandleKeyInput(key);
            UpdateLabel();

            IJavaScriptExecutor jse = SpotifyCLI.driver!;
            Thread.Sleep(100);
            jse.ExecuteScript("arguments[0].scrollBy(0, 200);", SharedElements.GetTracklistScrollView());
        }

        // TODO: optimize
        public override void UpdateLabel()
        {
            for (int i = 0 - (entriesToDisplay / 2); i < entriesToDisplay; i++) 
            {
                int trackIndex = (CustomModulus(currentScrollValue + i, tracklistData.Count));

                TracklistItem val = tracklistData![trackIndex];

                string name = val.name!;
                string album = val.album!;
                string artists = val.artists!;

                Console.SetCursorPosition(2, 5 + (entriesToDisplay / 2) + i);
                StaticUtilities.ClearRow(5 + (entriesToDisplay / 2) + i);

                if (i == 0)
                {
                    Console.Write($"{1 + trackIndex}\t{name} by {artists} on {album}"); 
                }

                else 
                {
                    Console.Write(SpotifyCLI.ANSI_GRAY + "\t" + name + SpotifyCLI.ANSI_RESET);
                }
            }
        }

        private static int CustomModulus(int x, int m)
        {
            return (x % m + m) % m; // what the fuck
        }
    }
}