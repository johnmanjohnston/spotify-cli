using spotify_cli_cs;
using OpenQA.Selenium.Chrome;
using SpotifyAPI.Web;
using System;
using SpotifyAPI.Web.Auth;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using System.Text;

// Initialize 
// ChromeDriver driver = new();
// driver.Navigate().GoToUrl("https://example.com");

class SpotifyCLI
{
    // shared
    public static SpotifyClient? spotify;
    public static ChromeDriver? driver;

    // other
    private static string SHELL_EXECUTABLE = "cmd.exe";
    private static string PROGRAM_FILES_DIR = "C:/Users/USER/OneDrive/Desktop/nerd/spotify-cli/spotify-cli-cs/spotify-cli-cs";

    public static bool FRONTEND_ONLY = false;

    /// <summary>
    /// Runs Python program to generate access token, and writes it
    /// to the access_token file
    /// </summary>
    private static void WriteAccessToken() 
    {
        string authPythonCommand;
        authPythonCommand = $"/C python {PROGRAM_FILES_DIR}/auth.py";
        System.Diagnostics.Process.Start(SHELL_EXECUTABLE, authPythonCommand);
    }

    /// <summary>
    /// Reads access token from the access_token files and returns it
    /// </summary>
    private static string GetAccessToken() 
    {
        string retval = "";
        
        using (StreamReader sr = File.OpenText($"{PROGRAM_FILES_DIR}/access_token")) {
            retval = sr.ReadLine()!;
        }

        return retval;
    }

    private static Thread tickThread;
    private static bool running = true;
    private static int tickCount = 0;

    private static string ANSI_GRAY = "\u001b[38;2;107;107;107m";
    private static string ANSI_RESET = "\u001b[0m";

    private static void Main()
    {
        if (!FRONTEND_ONLY) {
            // initialize Spotify client
            WriteAccessToken();
            spotify = new(GetAccessToken());

            // initialize webdriver
            ChromeOptions options = new();
            options.AddArgument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data");
            options.AddArgument("profile-directory=Default");
            driver = new ChromeDriver(options);

            // configure other classes
            SharedElements.driver = driver;

            // initialization is complete; open Spotify
            driver.Navigate().GoToUrl("https://open.spotify.com");
            Thread.Sleep(5000);
        }

        tickThread = new Thread(() =>
        {
            while (running)
            {
                Thread.Sleep(100);
                Tick();
            }
        });

        tickThread.IsBackground = true;
        tickThread.Start();

        Console.Clear();
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (!(Console.KeyAvailable))
        {
            var keyData = Console.ReadKey(true);

            if (keyData.Key == ConsoleKey.Spacebar)     { Modify.TogglePlayPause(); }
            if (keyData.Key == ConsoleKey.RightArrow)   { Modify.SkipForward(); }
            if (keyData.Key == ConsoleKey.LeftArrow)    { Modify.SkipBack(); }
            if (keyData.Key == ConsoleKey.F)            { Modify.ToggleHeart(); }
            if (keyData.Key == ConsoleKey.S)            { Modify.ChangeShuffleMode(); }
            if (keyData.Key == ConsoleKey.R)            { Modify.ChangeRepeatMode(); }
        }

        return;
    }

    private static string currentPlaybackLabel;
    private static string playbackDetailsLabel;

    private static int KNOWN_WINDOW_HEIGHT;
    private static int KNOWN_WINDOW_WIDTH;

    private static void ClearRow(int row)
    {
        Console.SetCursorPosition(0, row);
        Console.Write(new String(' ', Console.BufferWidth));
    }

    private static string GetProgressBarText()
    {
        int barWidth = 20;
        string loaded = "\u001b[38;2;30;215;96m━\u001b[0m";
        string pending = "\u001b[38;2;36;36;36m━\u001b[0m";
        int charsToLoad = (int)(barWidth * Read.GetNormalizedSongProgress());

        string retval = "";

        for (int i = 0; i <= barWidth; i++)
        {
            if (i <= charsToLoad)
            {
                retval += loaded;
            }
            else { retval += pending; }
        }

        return retval;
    }

    private static void RedrawCurrentlyPlaying()
    {
        ClearRow(Console.BufferHeight - 5);
        Console.SetCursorPosition(2, Console.BufferHeight - 5);
        Console.Write(Read.GetCurrentlyPlaying());
        currentPlaybackLabel = Read.GetCurrentlyPlaying();
    }

    private static void RedrawPlaybackDetails()
    {
        ClearRow(Console.BufferHeight - 4);
        Console.SetCursorPosition(2, Console.BufferHeight - 4);
        Console.Write(ANSI_GRAY + Read.GetPlaybackDetails() + ANSI_RESET);
        playbackDetailsLabel = Read.GetPlaybackDetails();
    }

    private static void RedrawProgressBar()
    {
        Console.SetCursorPosition(2, Console.BufferHeight - 3);
        Console.Write(GetProgressBarText());
    }

    private static void Tick()
    {
        tickCount++;

        RedrawProgressBar();

        if (currentPlaybackLabel != Read.GetCurrentlyPlaying())
        {
            RedrawCurrentlyPlaying();
        }

        if (playbackDetailsLabel != Read.GetPlaybackDetails())
        {
            RedrawPlaybackDetails();
        }

        if (KNOWN_WINDOW_HEIGHT != Console.BufferHeight || KNOWN_WINDOW_WIDTH != Console.BufferWidth)
        {
            Console.Clear();
            KNOWN_WINDOW_HEIGHT = Console.BufferHeight;
            KNOWN_WINDOW_WIDTH = Console.BufferWidth;

            RedrawCurrentlyPlaying();
            RedrawPlaybackDetails();
        }
    }
}