namespace spotify_cli_cs.Components
{
    public class ListView : TUIBaseComponent
    {
        private int displayedEntries = 5; // how many entires we can display at once
        private int currentScrollValue; // how far we have scrolled
        private List<KeyValuePair<string, string>> playlistData;

        public ListView(int x, int y) : base(x, y)
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
                currentScrollValue %= playlistData.Count;
            }

            else if (key == ConsoleKey.DownArrow) 
            {
                currentScrollValue++;
                currentScrollValue %= playlistData.Count;
            }

            if (currentScrollValue < 0) { currentScrollValue = playlistData.Count; }

            this.UpdateLabel();
        }

        public void UpdateLabel() 
        {
            int orgX = Console.GetCursorPosition().Left;
            int orgY = Console.GetCursorPosition().Top;

            currentScrollValue %= playlistData.Count;

            Utility.StaticUtilities.ClearRow(yPos);
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(playlistData[currentScrollValue].Value);

            Console.SetCursorPosition(orgX, orgY);
        }
    }
}