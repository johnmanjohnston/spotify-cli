using OpenQA.Selenium;
using spotify_cli_cs.Utility;
using SpotifyAPI.Web;

namespace spotify_cli_cs.Components
{
    public class ListView : TUIBaseComponent
    {
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

            else if (key == ConsoleKey.Enter)
            {
                // add song to playlist
                IPlayableItem curPlayingInfo = SpotifyCLI.spotify!.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result.Item;
                FullTrack track = (FullTrack)curPlayingInfo;
                string trackUri = track.Uri;

                string playlistUri = playlistData[(CustomModulus(currentScrollValue, playlistData.Count))].Key;

                var a = SpotifyCLI.spotify.Playlists.AddItems(
                    playlistUri.Split(":")[2],
                    new PlaylistAddItemsRequest(
                        new List<string>() { trackUri }
                    )
                );

                SpotifyCLI.DrawNotificationLabel($"\"{track.Name}\" added to \"{Trunacate(playlistData[(CustomModulus(currentScrollValue, playlistData.Count))].Value)}\"");
            }

            else return; // don't continue any further if it's a keybind we aren't doing anything for

            
            /*
            IPlayableItem __curPlayingInfo = SpotifyCLI.spotify!.Player.GetCurrentlyPlaying(new PlayerCurrentlyPlayingRequest()).Result.Item;
            FullTrack __track = (FullTrack)__curPlayingInfo;
            string __trackUri = __track.Uri;
            string __playlistUri = playlistData[(CustomModulus(currentScrollValue, playlistData.Count))].Key;

            var playlistID = SpotifyCLI.spotify.Playlists.Get(__playlistUri.Split(":")[2]).Result.Id;

            Read.SongInPlaylist(__trackUri, playlistID.ToString());
            */

            this.UpdateLabel();
        }

        private static int CustomModulus(int x, int m)
        {
            return (x % m + m) % m;
        }

        private string Trunacate(string s)
        {
            string retval = s;
            if (retval.Length > Console.WindowWidth - xPos)
            {
                retval = retval.Substring(0, Console.WindowWidth - xPos - 5) + "...";
            }
            return retval;
        }

        public void UpdateLabel() 
        {
            // int orgX = Console.GetCursorPosition().Left;
            // int orgY = Console.GetCursorPosition().Top;

            // Utility.StaticUtilities.ClearRow(yPos);
            string val = playlistData[(CustomModulus(currentScrollValue, playlistData.Count))].Value;

            // clear space
            SpotifyCLI.ClearRow(yPos, xPos, Console.WindowWidth - xPos);
            SpotifyCLI.ClearRow(yPos - 1, xPos, Console.WindowWidth - xPos);
            SpotifyCLI.ClearRow(yPos + 1, xPos, Console.WindowWidth - xPos);

            // write prev, current, and next values
            // current
            Console.SetCursorPosition(xPos, yPos);
            Console.Write(Trunacate(val));

            // previous
            Console.SetCursorPosition(xPos, yPos - 1);
            Console.Write(StaticUtilities.ANSI_GRAY + Trunacate(playlistData[(CustomModulus(currentScrollValue - 1, playlistData.Count))].Value) + StaticUtilities.ANSI_RESET);

            // next
            Console.SetCursorPosition(xPos, yPos + 1);
            Console.Write(StaticUtilities.ANSI_GRAY + Trunacate(playlistData[CustomModulus(currentScrollValue + 1, playlistData.Count)].Value) + StaticUtilities.ANSI_RESET);

            // Console.SetCursorPosition(orgX, orgY);
        }
    }
}