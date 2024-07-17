using OpenQA.Selenium;
using spotify_cli_cs.Models;
using spotify_cli_cs.Utility;
using spotify_cli_cs.Components.Core;

using static spotify_cli_cs.AdditionalData.AdditionalData;

namespace spotify_cli_cs.Components
{
    public class UserLibraryListView : BaseScrollView
    {
        public List<KeyValuePair<string, string>>? libData = new(); // format, <uri, name>
        public int entiresToDisplay; // assigned in Program.cs in OnTerminalResize()
        public UserLibraryListView(int x = 0, int y = 0) : base(x, y) { }

        public override void HandleKeyInput(ConsoleKey key)
        {
            base.HandleKeyInput(key);

            if (key == ConsoleKey.Enter)
            {
                Modify.GoToItemWithUri(libData![(CustomModulus(currentScrollValue, libData.Count))].Key, SpotifyCLI.driver!);
                // return;

                var item = (libData![(CustomModulus(currentScrollValue, libData.Count))].Key, SpotifyCLI.driver!);
                int itemType = -1;

                if (item.Key.Contains("album")) itemType = (int)DataMap.ALBUM;
                if (item.Key.Contains("playlist")) itemType = (int)DataMap.PLAYLIST;

                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        if (itemType == (int)DataMap.PLAYLIST)
                        {
                            List<TracklistItem> tracklistData = new();
                            List<IWebElement> list = SharedElements.CurrentTracklistSongChunk();

                            Thread.Sleep(500);

                            for (int i1 = 0; i1 < list.Count; i1++)
                            {
                                IWebElement? s = list[i1];
                                var titleElement = s.FindElements(By.XPath(".//a[@data-testid='internal-track-link']"))[0];
                                var albumElement = s.FindElements(By.XPath(".//a[@class='standalone-ellipsis-one-line'][@draggable='true']"))[0];

                                //Console.SetCursorPosition(2, i1 + 4);
                                //Console.WriteLine(titleElement.Text);

                                tracklistData.Add(new TracklistItem()
                                {
                                    name = titleElement.Text,
                                    album = albumElement.Text,
                                });

                                StaticUtilities.DBG(titleElement.Text);
                            }

                            SpotifyCLI.UpdateAndOpenTracklistView(tracklistData);

                            break;
                        }

                        if (itemType == (int)DataMap.ALBUM) 
                        {
                            // top-sentinel
                            IWebElement? wrapper = SharedElements.driver!.FindElement(By.XPath("//div[@data-testid='top-sentinel']"));
                            var tracks = wrapper.FindElements(By.XPath("//div[@data-testid='tracklist-row']//div[@data-encore-id='text' and @dir='auto']"));

                            foreach (var track in tracks) 
                            {
                                StaticUtilities.DBG(track.Text);
                            }
                        }

                    } catch (Exception) { Thread.Sleep(10); continue;  }
                }
            }

            this.UpdateLabel();
        }

        public override void OnBlur()
        {
        }

        public override void OnFocus()
        {
            // throw new NotImplementedException();
        }

        public override void UpdateLabel()
        {
            for (int i = 0 - (entiresToDisplay / 2); i < entiresToDisplay; i++)
            {
                string val = libData![(CustomModulus(currentScrollValue + i, libData.Count))].Value;
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
            return (x % m + m) % m; // what the fuck
        }
    }
}