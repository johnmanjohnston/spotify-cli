using SpotifyAPI;

namespace spotify_cli_cs.Utility
{
    public static class StaticUtilities 
    {
        public static void ClearRow(int row)
        {
            Console.SetCursorPosition(0, row);

            Console.Write(new String(' ', Console.WindowWidth));
        }
    }
}