using spotify_cli_cs.Utility;

namespace spotify_cli_cs.Components.Core;

public class TextInputField : TUIBaseComponent
{
    public TextInputField(int x = 0, int y = 0) : base(x, y) { }

    protected string? content; // content of the input field of this instance
    public override void HandleKeyInput(ConsoleKey key)
    {
        System.Diagnostics.Debug.WriteLine("incoming key: " + key.ToString());

        // append chars
        string c = key.ToString();
        if (c.Length < 2)
        {
            content += c;
        }

        // misc
        if (key == ConsoleKey.Backspace)
        {
            if (content!.Length > 0) // only trim the ending if the length if sufficient, otherwise you'll get an exception
                content = content[..^1];
        }

        else if (key == ConsoleKey.Spacebar)
        {
            content += " ";
        }

        else if (key == ConsoleKey.Enter)
        {
            this.OnEnter();
        }

        this.UpdateLabel();
    }

    public virtual void OnEnter()
    {
        System.Diagnostics.Debug.WriteLine("OnEnter() called on an instance of TextInputField");
    }

    public void UpdateLabel()
    {
        StaticUtilities.ClearRow(yPos);

        Console.SetCursorPosition(xPos, yPos);
        Console.Write(content + "_"); // the "_" represents a cursor
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