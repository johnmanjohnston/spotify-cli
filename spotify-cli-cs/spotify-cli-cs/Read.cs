using OpenQA.Selenium.DevTools.V116.Debugger;
using SpotifyAPI.Web;
using Swan;
using System;
using System.Runtime.CompilerServices;

namespace spotify_cli_cs 
{
    static class Read
    {
        private static void l(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }

        public static char GetHeartedStatus() 
        {
            if (SpotifyCLI.FRONTEND_ONLY) return '!';

            char HEARTED = '♥';
            char UNHEARTED = '♡';
         
            try
            {
                bool isHearted = SharedElements.GetHeartButton().GetAttribute("aria-checked").ToLower() == "true";

                return isHearted ? HEARTED : UNHEARTED;
            }

            catch
            {
                return '-';
            }
        }

        public static string GetPlaybackTimeInfo()
        {
            if (SpotifyCLI.FRONTEND_ONLY) return "read.getPlaybackTimeInfo() called with frontend only enabled";

            try
            {
                string pos = SharedElements.GetPlaybackPositionDiv().Text;
                string duration = SharedElements.GetPlaybackDurationDiv().Text.TrimStart('-');

                string retval = $"{pos} / {duration}";

                return retval;
            }

            catch
            {
                return "Loading...";
            }
        }

        public static string GetCurrentlyPlaying()
        {
            if (SpotifyCLI.FRONTEND_ONLY) return "Read.GetCurrentlyPlaying() and frontend only is enabeld";

            try {
                string retval = $"{SharedElements.GetSongNameLink().Text} - {SharedElements.GetArtistNameLink().Text}";
                return retval;
            } catch (Exception e)
            {
                l("exception for getting cur playing");
                l(e.Message);
                return "Loading...";
            }
        }

        public static string GetPlaybackDetails()
        {
            if (SpotifyCLI.FRONTEND_ONLY) return "Read.GetPlaybackDetails() with frontend only is enabled";

            try
            {
                string shuffleState = SharedElements.GetShuffleButton().GetAttribute("aria-checked").ToLower() == "true" ? "on" : "off";
                string repeatState = "";

                switch(SharedElements.GetRepeatButton().GetAttribute("aria-checked"))
                {
                    case "mixed":
                        repeatState = "song";
                        break;
                    case "true":
                        repeatState = "list";
                        break;
                    case "false":
                        repeatState = "off";
                        break;
                }

                return $"Shuffle: {shuffleState} - Repeat: {repeatState}";
            } 
            
            catch (Exception)
            {
                l("exception to getting palyback details");
                return "Loading...";
            }
        }
        
        public static float GetNormalizedSongProgress()
        {
            if (SpotifyCLI.FRONTEND_ONLY) return .5f;

            try
            {
                string progressBarStyle = SharedElements.GetProgressBarDiv().GetAttribute("style");
                var progress = Convert.ToDecimal(progressBarStyle.Split(": ")[1].Split("%")[0]);
                float retval = (float)progress / 100.0f;
                return retval;
            } catch (Exception) {
                l("exception for getting normalized song progress");
                return 0.0f; 
            }
        }

        // Spotify API data
        // private static List<KeyValuePair<string, string>> userPlaylists = new(); // in the format <uri, name>

        /// <summary>
        /// Returns user playlist URI and name in a list of key value pairs for the logged in user.
        /// If FRONTEND_ONLY is enabled, 5 demo URIs and names in a list of key value pairs are returned.
        /// </summary>
        public static List<KeyValuePair<string, string>> GetUserPlaylists() 
        {
            List<KeyValuePair<string, string>> retval = new();

            PlaylistCurrentUsersRequest req = new() 
            {
                Limit = 50,
            };

            if (!SpotifyCLI.FRONTEND_ONLY)
            {
                SpotifyClient spotify = SpotifyCLI.spotify!;
                var playlists = spotify!.Playlists.CurrentUsers(req).Result.Items;

                for (int i = 0; i < playlists!.Count; i++)
                {
                    if (playlists[i].Owner.Uri != SpotifyCLI.userUri) continue;

                    retval.Add(new KeyValuePair<string, string>(playlists[i].Uri, playlists[i].Name));
                }
            }

            else
            {
                for (int i = 0; i < 5; i++)
                {
                    retval.Add(new KeyValuePair<string, string>($"uri{i}", $"playlist {i}"));
                }
            }

            return retval;           
        }

        private static string GetPlaylistIDFromURI(string uri)
        {
            string retval = "";

            var playlist = SpotifyCLI.spotify.Playlists.Get(uri.Split(":")[2]);
            retval = playlist.Result.Id;

            return retval;
        }

        public static bool SongInPlaylist(string songUri, string playlistId)
        {
            var playlist = SpotifyCLI.spotify?.Playlists.Get(playlistId).Result;
            int numSongs = (int)playlist!.Tracks!.Total!;
            int chunks = Floor(numSongs, 100);

            List<Task<bool>> tasks = new();

            for (int i = 0; i <= chunks; i++)
            {
                var req = new PlaylistGetItemsRequest()
                {
                    Offset = i * 100
                };

                tasks.Add(Task.Run(() =>
                {
                    var playlistItems = SpotifyCLI.spotify?.Playlists.GetItems(playlistId, req).Result;
                    if (playlistItems == null || playlistItems.Items == null)
                    {
                        return false;
                    }

                    foreach (var item in playlistItems.Items)
                    {
                        FullTrack curTrack = (FullTrack)item.Track;

                        if (curTrack.Uri == songUri)
                        {
                            return true;
                        }
                    }
                    return false;
                }));
            }

            Task.WaitAll(tasks.ToArray());
            return tasks.Any(t => t.Result);
        }


        // https://stackoverflow.com/questions/28059655/floored-integer-division
        private static int Floor(int a, int b)
        {
            return (a / b - Convert.ToInt32(((a < 0) ^ (b < 0)) && (a % b != 0)));
        }
    }
}