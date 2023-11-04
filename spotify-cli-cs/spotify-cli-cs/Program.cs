using spotify_cli_cs;
using OpenQA.Selenium.Chrome;
using SpotifyAPI.Web;
using System;
using SpotifyAPI.Web.Auth;
using System.IO;
using System.Threading;
using OpenQA.Selenium;
using System.Text;
using OpenQA.Selenium.DevTools.V116.Network;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using spotify_cli_cs.Components;
using OpenQA.Selenium.DevTools.V116.Page;
using spotify_cli_cs.Utility;
using OpenQA.Selenium.DevTools.V116.DOM;

// Initialize 
// ChromeDriver driver = new();
// driver.Navigate().GoToUrl("https://example.com");

class SpotifyCLI
{
    // shared
    public static SpotifyClient? spotify;
    public static ChromeDriver? driver;

    // other
    private static string SHELL_EXECUTABLE = "powershell.exe";
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

    private static Thread? tickThread;
    private static bool running = true;
    private static int tickCount = 0;

    private static string splashScreenArt =
@"

                 _   _  __                  _ _ 
 ___ _ __   ___ | |_(_)/ _|_   _        ___| (_)
/ __| '_ \ / _ \| __| | |_| | | |_____ / __| | |
\__ \ |_) | (_) | |_| |  _| |_| |_____| (__| | |
|___/ .__/ \___/ \__|_|_|  \__, |      \___|_|_|
    |_|                    |___/                

";

    private static void DisplaySplashScreen()
    {
        int width = Console.WindowWidth;
        System.Diagnostics.Debug.WriteLine("debug log test");

        for (int i = 0; i < (Console.WindowHeight / 2) - 10; i++)
        {
            Console.WriteLine();
        }

        string[] lines = splashScreenArt.Split('\n');
        foreach (string line in lines)
        {
            int leftPadding = (width - line.Length) / 2;
            Console.WriteLine(line.PadLeft(leftPadding + line.Length));
        }
    }

    private static void DisplaySplashScreenLoadingMessage(string s)
    {
        //ClearRow((Console.WindowHeight / 2) + 4);
        StaticUtilities.ClearRow((Console.WindowHeight / 2) + 4);
        Console.SetCursorPosition((Console.WindowWidth / 2) - s.Length / 2, (Console.WindowHeight / 2) + 4);
        Console.WriteLine(s + '\n');
    }

    private static void Main()
    {
        DisplaySplashScreen();
        DisplaySplashScreenLoadingMessage("Loading");

        if (!FRONTEND_ONLY) {
            // initialize Spotify client
            DisplaySplashScreenLoadingMessage("Preparing Spotify authentication");
            WriteAccessToken();
            spotify = new(GetAccessToken());
            Thread.Sleep(2000);

            // initialize webdriver
            DisplaySplashScreenLoadingMessage("Preparing driver");
            ChromeOptions options = new();
            options.AddArgument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data");
            options.AddArgument("profile-directory=Default");

            // ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            // service.HideCommandPromptWindow = true; // hide all logs from the driver

            // driver = new ChromeDriver(service, options);
            driver = new ChromeDriver(options); // InvalidOperationException if Chrome already opened

            DisplaySplashScreenLoadingMessage("Assigning helper class variables");
            // configure other classes
            SharedElements.driver = driver;

            DisplaySplashScreenLoadingMessage("Opening Spotify");
            // initialization is complete; open Spotify
            driver.Navigate().GoToUrl("https://open.spotify.com");
            Thread.Sleep(4000);
        }


        Console.Clear();
        Console.CursorVisible = false;
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Initialize();

        DisplaySplashScreenLoadingMessage("Preparing ticking thread");
        tickThread = new Thread(() =>
        {
            while (running)
            {
                Thread.Sleep(40);
                Tick();
            }
        });
        tickThread.IsBackground = true;
        tickThread.Start();


        while (true)
        {
            if (!Console.KeyAvailable) continue;
            
            var keyData = Console.ReadKey(true);

            if (FOCUSED != null)
            {
                PENDING_COMPONENT_KEY = keyData.Key;
                // FOCUSED.HandleKeyInput(keyData.Key);
            }

            if (keyData.Key == ConsoleKey.Spacebar) { Modify.TogglePlayPause(); }
            else if (keyData.Key == ConsoleKey.RightArrow) { Modify.SkipForward(); }
            else if (keyData.Key == ConsoleKey.LeftArrow) { Modify.SkipBack(); }
            else if (keyData.Key == ConsoleKey.F) { Modify.ToggleHeart(); }
            else if (keyData.Key == ConsoleKey.S) { Modify.ChangeShuffleMode(); }
            else if (keyData.Key == ConsoleKey.R) { Modify.ChangeRepeatMode(); }

            else if (keyData.Key == ConsoleKey.P) { Console.Clear(); }
            else if (keyData.Key == ConsoleKey.O) { Console.CursorVisible = false; }

            else if (keyData.Key == ConsoleKey.Escape)
            {
                if (!FRONTEND_ONLY) { driver!.Close(); }

                Console.Clear();
                Environment.Exit(0);
            }
        }
    }

    // labels
    private static string? currentPlaybackLabel;
    private static string? playbackDetailsLabel;

    // size
    private static int KNOWN_WINDOW_HEIGHT;
    private static int KNOWN_WINDOW_WIDTH;

    // colors
    private static string ANSI_GRAY = "\u001b[38;2;107;107;107m";
    private static string ANSI_SPOTIFY_GREEN = "\u001b[38;2;30;215;96m";
    private static string ANSI_DARK_GRAY = "\u001b[38;2;36;36;36m";
    private static string ANSI_RESET = "\u001b[0m";

    // margins
    private static int BOTTOM_BAR_MARGIN_LEFT = 3;
    private static int BOTTOM_BAR_MARGIN_BOTTOM = 3;

    // Spotify data
    public static List<KeyValuePair<string, string>> userPlaylists = new(); // in the format <uri, name>
    public static string? userUri;

    // component data
    private static ListView? playlistView;
    private static TUIBaseComponent? FOCUSED;

    private static void Initialize()
    {
        userUri = spotify?.UserProfile.Current().Result.Uri;
        userPlaylists = Read.GetUserPlaylists();

        playlistView = new();

        FOCUSED = playlistView; // by default
        FOCUSED.OnFocus();
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

    private static void DrawLineBorderThing(int row)
    {
        Console.SetCursorPosition(0, row);
        Console.Write(ANSI_DARK_GRAY);
        Console.Write(new string('_', Console.WindowWidth));
        Console.Write(ANSI_RESET);
    }

    private static int progressBarWidth = 20;
    private static string GetProgressBarText()
    {
        string loaded = $"{ANSI_SPOTIFY_GREEN}━{ANSI_RESET}";
        string pending = $"{ANSI_DARK_GRAY}━{ANSI_RESET}";
        int charsToLoad = (int)(progressBarWidth * Read.GetNormalizedSongProgress());

        string retval = "";

        for (int i = 0; i <= progressBarWidth; i++)
        {
            if (i <= charsToLoad)
            {
                retval += loaded;
            }
            else { retval += pending; }
        }

        return retval;
    }

    // Redraw the heart SEPERATELY from the current playback label.
    // We do this so that we don't have to redraw the entire label
    // just to update one heart.
    private static void RedrawHeartedStatus() 
    {
        if (string.IsNullOrEmpty(currentPlaybackLabel)) return;

        Console.SetCursorPosition(currentPlaybackLabel.Length + 4, Console.WindowHeight - 3 - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(Read.GetHeartedStatus());
    }

    private static void RedrawCurrentlyPlaying()
    {
        // ClearRow(Console.WindowHeight - 3 - BOTTOM_BAR_MARGIN_BOTTOM);

        // if we clear the entire row, we also clear the "Add songs to playlist" header above
        // the playlistView which is drawn in OnResizeTerminal(). We take care not to erase
        // that header over here
        int? charsToReplace = null;
        if (playlistView != null)
        {
            charsToReplace = currentPlaybackLabel?.Length + BOTTOM_BAR_MARGIN_LEFT + 2; // +2 chars to accommadate for the hearted status
        }

        StaticUtilities.ClearRow(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 3, charsToReplace: charsToReplace);
        Console.SetCursorPosition(BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - 3 - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(Read.GetCurrentlyPlaying());
        currentPlaybackLabel = Read.GetCurrentlyPlaying();
    }

    private static void RedrawPlaybackDetails()
    {
        //ClearRow(Console.WindowHeight - 2 - BOTTOM_BAR_MARGIN_BOTTOM);
        StaticUtilities.ClearRow(Console.WindowHeight - 2 - BOTTOM_BAR_MARGIN_BOTTOM, 0, charsToReplace: Read.GetPlaybackDetails().Length + 1);
        Console.SetCursorPosition(BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - 2 - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(ANSI_GRAY + Read.GetPlaybackDetails() + ANSI_RESET);
        playbackDetailsLabel = Read.GetPlaybackDetails();
    }

    private static void RedrawProgressBar()
    {
        Console.SetCursorPosition(BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - 1 - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(GetProgressBarText());

        // clean up any text drawn in the wrong spot due to cursor conflicts
        /* Console.SetCursorPosition(BOTTOM_BAR_MARGIN_LEFT + progressBarWidth, Console.WindowHeight - 1 - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(new String(' ', Console.WindowWidth - (BOTTOM_BAR_MARGIN_LEFT + progressBarWidth)));

        if (!string.IsNullOrEmpty(currentPlaybackLabel))
        {
            Console.SetCursorPosition(currentPlaybackLabel.Length + 5, Console.WindowHeight - 3 - BOTTOM_BAR_MARGIN_BOTTOM);
            Console.Write(new String(' ', Console.WindowWidth - (currentPlaybackLabel.Length + 5)));
        } */
    }

    private static void RedrawPlaybackTimeInfo()
    {
        Console.SetCursorPosition(BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(ANSI_GRAY + Read.GetPlaybackTimeInfo() + ANSI_RESET);

        // clean up any text drawn in the wrong spot due to cursor conflicts
        Console.SetCursorPosition(0, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(new String(' ', BOTTOM_BAR_MARGIN_LEFT));
    }

    private static ConsoleKey? PENDING_COMPONENT_KEY;

    private static void HandlePendingComponentInput() 
    {
        if (PENDING_COMPONENT_KEY != null && FOCUSED != null)
        {
            FOCUSED.HandleKeyInput((ConsoleKey)PENDING_COMPONENT_KEY);
            PENDING_COMPONENT_KEY = null;
        }
    }

    private static void Tick()
    {
        // We call HandlePendingComponentInput() a lot to reduce the delay between key presses.
        // We don't carry out the key function for the component immediately when we get the input,
        // as it can mess with the cursor position and can change where some text gets rendered.

        tickCount++;
    HandlePendingComponentInput();

        RedrawProgressBar();
        RedrawPlaybackTimeInfo();
        
    HandlePendingComponentInput();

        if (currentPlaybackLabel != Read.GetCurrentlyPlaying())
        {
            RedrawCurrentlyPlaying();
        }

    HandlePendingComponentInput();


        if (playbackDetailsLabel != Read.GetPlaybackDetails())
        {
            RedrawPlaybackDetails();
        }

    HandlePendingComponentInput();
        
        RedrawHeartedStatus(); // redraw hearted status AFTER RedrawCurrentlyPlaying()
    
    HandlePendingComponentInput();

        // if there's a change in the width/height we think it is,
        // and the actual width/height, then it means the terminal was resized
        if (KNOWN_WINDOW_HEIGHT != Console.WindowHeight || KNOWN_WINDOW_WIDTH != Console.WindowWidth)
        {
            // clear terminal, and assign new width and height
            Console.Clear();
            KNOWN_WINDOW_HEIGHT = Console.WindowHeight;
            KNOWN_WINDOW_WIDTH = Console.WindowWidth;

            OnResizeTerminal();
        }

    HandlePendingComponentInput();
    }

    private static void OnResizeTerminal()
    {
        // redraw everything
        RedrawCurrentlyPlaying();
        RedrawPlaybackDetails();

        DrawLineBorderThing(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 5);
        
        // reposition playlist view
        if (playlistView != null)
        {
            playlistView.yPos = Console.WindowHeight - 1 - BOTTOM_BAR_MARGIN_BOTTOM;
            playlistView.xPos = Console.WindowWidth - (Console.WindowWidth / 3);
        }

        // draw title for playlist view
        if (playlistView != null)
        {
            Console.SetCursorPosition(playlistView.xPos, playlistView.yPos - 2);
            Console.Write(ANSI_GRAY + "Add current song to playlist" + ANSI_RESET);
            // To underline use this ANSI: Console.Write("\u001b[4masdfasdf\u001b[0m");
        }
        playlistView?.UpdateLabel();
        DrawNotificationLabel("Notifications will be displayed here.");
    }

    public static void DrawNotificationLabel(string s)
    {
        StaticUtilities.ClearRow(1);
        Console.SetCursorPosition(2, 1);
        Console.Write(ANSI_GRAY + s + ANSI_RESET);
    }
}