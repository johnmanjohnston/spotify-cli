namespace spotify_cli_cs.Components
{
    public abstract class TUIBaseComponent
    {
        public abstract void OnFocus();
        public abstract void OnBlur();
        public abstract void HandleKeyInput(ConsoleKey key);

        public int xPos;
        public int yPos;
        public TUIBaseComponent(int x, int y) 
        {
            this.xPos = x;
            this.yPos = y;
        }
    }
}