using SpotifyAPI;

namespace spotify_cli_cs.Utility
{
    public static class StaticUtilities 
    {
        public static void ClearRow(int row, int offset = 0, int? charsToReplace = null)
        {
            charsToReplace ??= Console.WindowWidth;
            int orgX = Console.GetCursorPosition().Left;
            int orgY = Console.GetCursorPosition().Top;

            Console.SetCursorPosition(offset, row);
            Console.Write(new String(' ', (int)charsToReplace));

            Console.SetCursorPosition(orgX, orgY);
        }
    }
}