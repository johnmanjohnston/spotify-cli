using OpenQA.Selenium.DevTools.V116.Storage;
using spotify_cli_cs.Utility;
using System.Linq;

namespace spotify_cli_cs.Components
{
    public class UserLibraryListView : TUIBaseComponent
    {
        private int currentScrollValue; // how far we have scrolled
        public List<KeyValuePair<string, string>>? libData = new(); // format, <uri, name>
        public int entiresToDisplay; // assigned in Program.cs in OnTerminalResize()
        public UserLibraryListView(int x = 0, int y = 0) : base(x, y) { }

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
            else return;

            this.UpdateLabel();
        }

        public override void OnBlur()
        {
        }

        public override void OnFocus()
        {
            // throw new NotImplementedException();
        }

        public void UpdateLabel()
        {
            for (int i = 0 - (entiresToDisplay / 2); i < entiresToDisplay; i++)
            {
                string val = libData[(CustomModulus(currentScrollValue + i, libData.Count))].Value;
                Console.SetCursorPosition(2, 5 + (entiresToDisplay / 2) + i);
                StaticUtilities.ClearRow(5 + (entiresToDisplay / 2) + i);
                
                if (i == 1)
                {
                    Console.Write(val);
                } else
                {
                    Console.Write(SpotifyCLI.ANSI_GRAY + val + SpotifyCLI.ANSI_RESET);
                }
            }
        }

        private static int CustomModulus(int x, int m)
        {
            return (x % m + m) % m;
        }
    }
}