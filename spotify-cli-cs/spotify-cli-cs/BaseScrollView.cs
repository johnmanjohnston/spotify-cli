using spotify_cli_cs.Components;

namespace spotify_cli_cs
{
    public abstract class BaseScrollView : TUIBaseComponent
    {
        protected int currentScrollValue = 0; // how far we have scrolled

        public abstract void UpdateLabel();

        protected BaseScrollView(int x, int y) : base(x, y)
        {
            this.xPos = x;
            this.yPos = y;
        }

        protected void PageScrollDown() 
        {
            for (int i = 0; i < 5; i++) 
            {
                this.currentScrollValue += 2;
                Thread.Sleep(1);
                UpdateLabel();
            }
        }

        protected void PageScrollUp()
        {
            for (int i = 0; i < 5; i++)
            {
                this.currentScrollValue -= 2;
                Thread.Sleep(1);
                UpdateLabel();
            }
        }

        public override void HandleKeyInput(ConsoleKey key) 
        {
            if (key == ConsoleKey.DownArrow)
            {
                this.currentScrollValue++;
            }

            else if (key == ConsoleKey.UpArrow)
            {
                this.currentScrollValue--;
            }

            else if (key == ConsoleKey.PageUp)
            {
                PageScrollUp();
            }

            else if (key == ConsoleKey.PageDown) 
            {
                PageScrollDown();
            }

            UpdateLabel();
        }
    }
}
