using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V116.Storage;
using spotify_cli_cs.Models;
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

            else if (key == ConsoleKey.PageUp)
            {
                currentScrollValue -= 10;
            }

            else if (key == ConsoleKey.PageDown)
            {
                currentScrollValue += 10;
            }

            else if (key == ConsoleKey.Enter)
            {
                Modify.GoToItemWithUri(libData![(CustomModulus(currentScrollValue, libData.Count))].Key, SpotifyCLI.driver!);
                return;
                Thread.Sleep(500);

                SpotifyCLI.driver!.FindElement(By.XPath("//div[@data-testid='playlist-tracklist']")).SendKeys(OpenQA.Selenium.Keys.PageDown);
                
                List<TracklistItem> tracklistData = new();
                foreach (var s in SharedElements.CurrentTracklistSongChunk())
                {
                    var titleElement = s.FindElements(By.XPath(".//a[@data-testid='internal-track-link']"))[0];
                    var albumElement = s.FindElements(By.XPath(".//a[@class='standalone-ellipsis-one-line'][@draggable='true']"))[0];

                    tracklistData.Add(new TracklistItem() 
                    {
                        name = titleElement.Text,
                        album = albumElement.Text,
                    });
                }

                SpotifyCLI.UpdateAndOpenTracklistView(tracklistData);
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
                
                if (i == 0)
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