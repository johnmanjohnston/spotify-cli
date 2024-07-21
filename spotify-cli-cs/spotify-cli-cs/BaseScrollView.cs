
namespace spotify_cli_cs.Components.Core;

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
            this.UpdateLabel();
        }
    }

    protected void PageScrollUp()
    {
        for (int i = 0; i < 5; i++)
        {
            this.currentScrollValue -= 2;
            Thread.Sleep(1);
            this.UpdateLabel();
        }
    }

    /// <summary>
    /// Handles input for basic scrolling functionality. Lets you scroll up and down using arrow keys,
    /// and page up/down keys. UpdateLabel() is NOT called from this function, however for the page
    /// up/down scrolling functionality, it triggers UpdateLabel()
    /// </summary>
    /// <param name="key"></param>
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
    }
}
