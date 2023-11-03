namespace spotify_cli_cs.Components
{
    public class ListView : TUIBaseComponent
    {
        private int displayedEntries = 5; // how many entires we can display at once
        private int currentScrollValue; // how far we have scrolled
        private List<KeyValuePair<string, string>> playlistData;

        public ListView(int x = 0, int y = 0) : base(x, y)
        {
            this.playlistData = SpotifyCLI.userPlaylists;
        }

        public override void OnFocus()
        {
            Console.Write("OnFocus() on ListView");   
        }

        public override void OnBlur()
        {
            throw new NotImplementedException();
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }

        public override void HandleKeyInput(ConsoleKey key)
        {
            if (key == ConsoleKey.UpArrow) 
            {
                currentScrollValue--;
            }

            else if (key == ConsoleKey.DownArrow) 
            {
                currentScrollValue++;
            }

            this.UpdateLabel();
        }

        private static int CustomModulus(int x, int m)
        {
            return (x % m + m) % m;
        }

        public void UpdateLabel() 
        {
            int orgX = Console.GetCursorPosition().Left;
            int orgY = Console.GetCursorPosition().Top;

            // Utility.StaticUtilities.ClearRow(yPos);
            string val = playlistData[(CustomModulus(currentScrollValue, playlistData.Count))].Value;

            if (val.Length > Console.WindowWidth - xPos) 
            {
                val = val.Substring(0, Console.WindowWidth - xPos - 5) + "...";
            }

            SpotifyCLI.ClearRow(yPos, xPos, Console.WindowWidth - xPos);

            Console.SetCursorPosition(xPos, yPos);
            Console.Write(val);

            Console.SetCursorPosition(orgX, orgY);
        }
    }
}