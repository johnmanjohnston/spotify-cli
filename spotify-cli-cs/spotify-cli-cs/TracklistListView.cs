using spotify_cli_cs.Components.Core;
using spotify_cli_cs.Utility;

namespace spotify_cli_cs.Components
{
    public class TracklistListView : BaseScrollView
    {
        public List<string>? trackNames = new(); // assign names of track to display to the user--when a song is selected, look at the index of the selected track and find the corresponding song using selenium
        private int entriesToDisplay = 10;

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
        }

        // TODO: optimize
        public override void UpdateLabel()
        {
            for (int i = 0 - (entriesToDisplay / 2); i < entriesToDisplay; i++) 
            {
                string val = trackNames![(CustomModulus(currentScrollValue + i, trackNames.Count))];

                Console.SetCursorPosition(2, 5 + (entriesToDisplay / 2) + i);
                StaticUtilities.ClearRow(5 + (entriesToDisplay / 2) + i);

                if (i == 0)
                {
                    Console.Write(SpotifyCLI.ANSI_GRAY + $"({1 + (CustomModulus(currentScrollValue + i, trackNames.Count))}) " + val + SpotifyCLI.ANSI_RESET);
                }

                else 
                {
                    Console.Write(val);
                }
            }
        }

        private static int CustomModulus(int x, int m)
        {
            return (x % m + m) % m; // what the fuck
        }
    }
}