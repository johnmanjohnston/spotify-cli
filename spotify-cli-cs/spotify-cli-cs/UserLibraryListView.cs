﻿using OpenQA.Selenium;
using spotify_cli_cs.Models;
using spotify_cli_cs.Utility;
using spotify_cli_cs.Components.Core;

using static spotify_cli_cs.AdditionalData.AdditionalData;
using SpotifyAPI.Web;

namespace spotify_cli_cs.Components
{
    public class UserLibraryListView : BaseScrollView
    {
        public List<KeyValuePair<string, string>>? libData = new(); // format, <uri, name>
        public int entiresToDisplay; // assigned in Program.cs in OnTerminalResize()
        public UserLibraryListView(int x = 0, int y = 0) : base(x, y) { }

        public override async void HandleKeyInput(ConsoleKey key)
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

                if (itemType == (int)DataMap.PLAYLIST) 
                {
                    // List<string> trackNames = new();
                    SpotifyCLI.tracklistListView.trackNames = new();

                    var playlistID = item.Key.Split(":")[2];
                    var playlist = SpotifyCLI.spotify?.Playlists.Get(playlistID).Result;
                    int numSongs = (int)playlist!.Tracks!.Total!;
                    int chunks = Read.Floor(numSongs, 100);

                    List<Task> tasks = new();

                    for (int i = 0; i <= chunks; i++)
                    {
                        StaticUtilities.DBG("getting songs for chunk number " + i);

                        var req = new PlaylistGetItemsRequest()
                        {
                            Offset = i * 100,
                            Limit = 100
                        };

                        tasks.Add(Task.Run(() => {
                            var res = SpotifyCLI.spotify.Playlists.GetItems(playlistID, req).Result;

                            for (var j = 0; j < res.Items.Count; j++) 
                            {
                                FullTrack track = (FullTrack)res.Items[j].Track;
                                StaticUtilities.DBG(track.Name);
                                //trackNames.Add(track.Name);
                                SpotifyCLI.tracklistListView.trackNames.Add(track.Name);
                            }

                            Thread.Sleep(100);
                        }));
                    }

                    //Thread.Sleep(3500);
                    Thread.Sleep(500);

                    SpotifyCLI.UpdateAndOpenTracklistView(null);
                }

                /*
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
                            IWebElement? wrapper = SharedElements.driver!.FindElement(By.XPath("//div[@data-testid='top-sentinel']"));
                            var tracks = wrapper.FindElements(By.XPath("//div[@data-testid='tracklist-row']//div[@data-encore-id='text' and @dir='auto']"));

                            foreach (var track in tracks) 
                            {
                                StaticUtilities.DBG(track.Text);
                            }
                        }

                    } catch (Exception) { Thread.Sleep(10); continue;  }
                }
                */
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