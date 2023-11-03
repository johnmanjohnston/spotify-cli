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

        public static string ANSI_GRAY = "\u001b[38;2;107;107;107m";
        public static string ANSI_SPOTIFY_GREEN = "\u001b[38;2;30;215;96m";
        public static string ANSI_DARK_GRAY = "\u001b[38;2;36;36;36m";
        public static string ANSI_RESET = "\u001b[0m";
    }
}