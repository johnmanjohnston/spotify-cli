using spotify_cli_cs.Utility;
using SpotifyAPI.Web;

namespace spotify_cli_cs.Components
{
    public class TracklistListView : TUIBaseComponent
    {
        private int currentScrollValue; // how far we have scrolled
        private List<KeyValuePair<string, string>> tracklistData;

        public TracklistListView(int x = 0, int y = 0) : base(x, y) { }

        public override void HandleKeyInput(ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow) 
            {
                currentScrollValue--;
            }

            else if (key == ConsoleKey.UpArrow)
            {
                currentScrollValue++;
            }

        }

        public override void OnBlur()
        {
            throw new NotImplementedException();
        }

        public override void OnFocus()
        {
            throw new NotImplementedException();
        }
    }
}