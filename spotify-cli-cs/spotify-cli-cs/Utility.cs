using spotify_cli_cs.Components;
using SpotifyAPI;

namespace spotify_cli_cs.Utility
{
    public static class StaticUtilities 
    {
        public static void DBG(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }

        public static void ClearRow(int row, int offset = 0, int? charsToReplace = null)
        {
            charsToReplace ??= Console.WindowWidth;
            int orgX = Console.GetCursorPosition().Left;
            int orgY = Console.GetCursorPosition().Top;

            Console.SetCursorPosition(offset, row);
            Console.Write(new String(' ', (int)charsToReplace));

            Console.SetCursorPosition(orgX, orgY);
        }

        public static void DrawVerticalLineDivisor(int column, int row, int? charsToDraw = null)
        {
            // TODO
            int orgX = Console.GetCursorPosition().Left;
            int orgY = Console.GetCursorPosition().Top;

            charsToDraw ??= Console.WindowHeight - row;

            for (int i = 0; i < charsToDraw; i++)
            {
                Console.SetCursorPosition(column, row + i);
                Console.Write(ANSI_DARK_GRAY + "|" + ANSI_RESET);

                Console.SetCursorPosition(orgX, orgY);
            }
        }

        public static string Trunacate(string s, int max)
        {
            if (s.Length <= max) return s;
            return s.Substring(0, max) + "…";
        }

        public static string ANSI_GRAY = "\u001b[38;2;107;107;107m";
        public static string ANSI_SPOTIFY_GREEN = "\u001b[38;2;30;215;96m";
        public static string ANSI_DARK_GRAY = "\u001b[38;2;36;36;36m";
        public static string ANSI_RESET = "\u001b[0m";
    }
}