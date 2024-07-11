using spotify_cli_cs;
using spotify_cli_cs.Components;
using spotify_cli_cs.Utility;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using SpotifyAPI.Web;
using spotify_cli_cs.Models;

// Initialize 
// ChromeDriver driver = new();
// driver.Navigate().GoToUrl("https://example.com");

class SpotifyCLI
{
    // shared
    public static SpotifyClient? spotify;
    public static EdgeDriver driver;

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
    private static int ticksSinceLastScreenResize = 0;

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

            for (int i = 0; i < 5; i++)
            {
                DisplaySplashScreenLoadingMessage("Attemping to authenticate with the Spotify API...");

                try
                {
                    spotify = new(GetAccessToken());
                    var t = spotify?.UserProfile.Current().Result.Uri;

                    DisplaySplashScreenLoadingMessage("Auth successful");
                    break;
                }
                catch { Thread.Sleep(1000); DisplaySplashScreenLoadingMessage("Auth failed.");  continue; }
            }
            
            Thread.Sleep(2000);

            var useEdge = true;

            if (!useEdge)
            {
                // initialize webdriver
                DisplaySplashScreenLoadingMessage("Preparing driver");
                ChromeOptions options = new();
                options.AddArgument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data");
                options.AddArgument("profile-directory=Default");

                // ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                // service.HideCommandPromptWindow = true; // hide all logs from the driver

                // driver = new ChromeDriver(service, options);
               // driver = new ChromeDriver(options); // InvalidOperationException if Chrome already opened

                DisplaySplashScreenLoadingMessage("Assigning helper class variables");
                // configure other classes
               // SharedElements.driver = driver;
            }

            else 
            {
                // C:\Users\USER\AppData\Local\Microsoft\Edge\User Data\Default
                EdgeOptions options = new();
                EdgeDriverService service = EdgeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;

                options.AddArgument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Microsoft\\Edge\\User Data");
                options.AddArgument("profile-directory=Default");

                driver = new EdgeDriver(service, options);
                SharedElements.driver = driver;
            }

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
                // HandlePendingComponentInput(); // REMOVE 
                // FOCUSED.HandleKeyInput(keyData.Key);
            }

            if (FOCUSED!.BLOCK_INPUT_FROM_OTHER_FUNCTIONALITY == false)
            {
                if (keyData.Key == ConsoleKey.Spacebar)
                {
                    Modify.TogglePlayPause();
                }

                else if (keyData.Key == ConsoleKey.RightArrow) { Modify.SkipForward(); }
                else if (keyData.Key == ConsoleKey.LeftArrow) { Modify.SkipBack(); }
                else if (keyData.Key == ConsoleKey.F) { Modify.ToggleHeart(); }
                else if (keyData.Key == ConsoleKey.S) { Modify.ChangeShuffleMode(); }
                else if (keyData.Key == ConsoleKey.R) { Modify.ChangeRepeatMode(); }
                else if (keyData.Key == ConsoleKey.M) { Modify.ToggleMute(); }

                else if (keyData.Key == ConsoleKey.P) { Console.Clear(); }
                else if (keyData.Key == ConsoleKey.O) { Console.CursorVisible = false; }

            }

            // this functionality is more important, so ignore any
            // instructions to block the orginal functionality
            if (keyData.Key == ConsoleKey.Escape)
            {
                // Exit and clean up
                if (!FRONTEND_ONLY) { driver!.Close(); }

                Console.Clear();
                Environment.Exit(0);
            }

            else if (keyData.Key == ConsoleKey.OemPeriod) { tabState = (Tab)((int)((tabState) + 1) % 3); PENDING_UPDATE_TAB_CONTENT = true; }
            else if (keyData.Key == ConsoleKey.OemComma) { tabState = (Tab)(((int)((tabState) - 1) + 3) % 3); PENDING_UPDATE_TAB_CONTENT = true; }

            else if (keyData.Key == ConsoleKey.Tab)
            {
                focusIndex++;
                focusIndex %= components.Count;
                FOCUSED!.OnBlur();
                FOCUSED = components[focusIndex];
                FOCUSED.OnFocus();
            }
        }
    }

    // labels
    private static string? currentPlaybackLabel;
    private static string? playbackDetailsLabel;
    private static string? nextSongLabel;

    // size
    private static int KNOWN_WINDOW_HEIGHT;
    private static int KNOWN_WINDOW_WIDTH;

    // colors
    public static string ANSI_GRAY = "\u001b[38;2;107;107;107m";
    public static string ANSI_SPOTIFY_GREEN = "\u001b[38;2;30;215;96m";
    public static string ANSI_DARK_GRAY = "\u001b[38;2;36;36;36m";
    public static string ANSI_RESET = "\u001b[0m";

    // margins
    private static int BOTTOM_BAR_MARGIN_LEFT = 3;
    private static int BOTTOM_BAR_MARGIN_BOTTOM = 3;

    // Spotify data
    public static List<KeyValuePair<string, string>> userPlaylists = new(); // in the format <uri, name> ONLY WHICH ONES USER OWNS
    public static List<KeyValuePair<string, string>> allUserSavedPlaylists = new(); // in the format <uri, name>
    public static string? userUri;

    // component data
    private static AddToPlaylistListView? playlistView;
    private static UserLibraryListView? userLibListView;
    private static SpotifySearchInputField? searchInputField;

    private static TUIBaseComponent? FOCUSED;
    private static bool PENDING_UPDATE_TAB_CONTENT = false;

    private static List<TUIBaseComponent> components = new();
    private static int focusIndex = 0;

    private enum Tab { 
        Library,
        Search, 
        Tracklist  // same tab format for playing albums/playlists
    };

    private static Tab tabState;
    private static List<TracklistItem> tracklist;

    private static void Initialize()
    {
        userUri = spotify?.UserProfile.Current().Result.Uri;
        userPlaylists = Read.GetUserPlaylists();
        allUserSavedPlaylists = Read.GetUserPlaylists(false);
        playlistView = new();
        userLibListView = new();
        searchInputField = new() { BLOCK_INPUT_FROM_OTHER_FUNCTIONALITY = true, xPos = 2, yPos = 5 };

        FOCUSED = userLibListView; // by default
        FOCUSED.OnFocus();

        tabState = Tab.Library;
        PENDING_UPDATE_TAB_CONTENT = true;

        components.Add(userLibListView);
        components.Add(playlistView);
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

    // Showing which songs were hearted was actually one feature
    // from Spotify which I ACTUALLY FUCKING LIKED. Thanks for 
    // removing it and not giving the option to revert, Spotify!
    /*
    private static void RedrawHeartedStatus() 
    {
        if (string.IsNullOrEmpty(currentPlaybackLabel)) return;

HandlePendingComponentInput();

        Console.SetCursorPosition(Read.GetCurrentlyPlaying((((Console.WindowWidth / 3) - (BOTTOM_BAR_MARGIN_LEFT + 6)) / 2) - 1).Length + 4, Console.WindowHeight - 3 - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(Read.GetHeartedStatus());
    }
    */

    private static void RedrawSavedStatusForCurrentSong() 
    {
        Console.SetCursorPosition(Read.GetCurrentlyPlaying((((Console.WindowWidth / 3) - (BOTTOM_BAR_MARGIN_LEFT + 6)) / 2) - 1).Length + 4, Console.WindowHeight - 3 - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(Read.GetSavedStatusForCurrentSong());
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
            charsToReplace = (Console.WindowWidth / 3);
        }

        StaticUtilities.ClearRow(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 3, charsToReplace: charsToReplace);
        Console.SetCursorPosition(BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - 3 - BOTTOM_BAR_MARGIN_BOTTOM);
        // Console.Write(StaticUtilities.Trunacate(Read.GetCurrentlyPlaying(), (Console.WindowWidth / 3) - (BOTTOM_BAR_MARGIN_LEFT + 6) ));
        Console.Write(Read.GetCurrentlyPlaying((( (Console.WindowWidth / 3) - (BOTTOM_BAR_MARGIN_LEFT + 6)) / 2 ) - 1));
        currentPlaybackLabel = Read.GetCurrentlyPlaying();
    }

    private static void RedrawPlaybackDetails()
    {
        //ClearRow(Console.WindowHeight - 2 - BOTTOM_BAR_MARGIN_BOTTOM);
        StaticUtilities.ClearRow(Console.WindowHeight - 2 - BOTTOM_BAR_MARGIN_BOTTOM, 0, charsToReplace: Read.GetPlaybackDetails().Length + 1 + BOTTOM_BAR_MARGIN_LEFT);

HandlePendingComponentInput();
        
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

// HandlePendingComponentInput(); // causes cursor conflicts

        // clean up any text drawn in the wrong spot due to cursor conflicts
        Console.SetCursorPosition(0, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM);
        Console.Write(new String(' ', BOTTOM_BAR_MARGIN_LEFT));
    }

    private static ConsoleKey? PENDING_COMPONENT_KEY;

    public static void HandlePendingComponentInput() 
    {
        if (PENDING_COMPONENT_KEY != null && FOCUSED != null)
        {
            FOCUSED.HandleKeyInput((ConsoleKey)PENDING_COMPONENT_KEY);
            PENDING_COMPONENT_KEY = null;
        }
    }

    private static void PrepareRedrawSongContext()
    {
        StaticUtilities.ClearRow(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 1, (Console.WindowWidth / 3) + 1, Console.WindowWidth / 3);
        Console.SetCursorPosition((Console.WindowWidth / 3) + BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 1);
        Console.Write("Loading...");
    }

    private static void Tick()
    {
        // We call HandlePendingComponentInput() a lot to reduce the delay between key presses.
        // We don't carry out the key function for the component immediately when we get the input,
        // as it can mess with the cursor position and can change where some text gets rendered.

        tickCount++;
        ticksSinceLastScreenResize++;

    HandlePendingComponentInput();

        if (PENDING_UPDATE_TAB_CONTENT) { DrawTabContent(); PENDING_UPDATE_TAB_CONTENT = false; }

    HandlePendingComponentInput(); UpdateTabOverheadPanel();

        RedrawProgressBar();

    HandlePendingComponentInput();

        RedrawPlaybackTimeInfo();
        
    HandlePendingComponentInput();

        if (currentPlaybackLabel != Read.GetCurrentlyPlaying())
        {
            RedrawCurrentlyPlaying();
        }

    HandlePendingComponentInput(); UpdateTabOverheadPanel();

        if (playbackDetailsLabel != Read.GetPlaybackDetails())
        {
            RedrawPlaybackDetails();
        }

    HandlePendingComponentInput(); UpdateTabOverheadPanel();

        DrawNextSongDetails();

    HandlePendingComponentInput();

        RedrawSavedStatusForCurrentSong();

    HandlePendingComponentInput();

        // if there's a change in the width/height we think it is,
        // and the actual width/height, then it means the terminal was resized
        if (KNOWN_WINDOW_HEIGHT != Console.WindowHeight || KNOWN_WINDOW_WIDTH != Console.WindowWidth)
        {
            // clear terminal, and assign new width and height
            Console.Clear();
            KNOWN_WINDOW_HEIGHT = Console.WindowHeight;
            KNOWN_WINDOW_WIDTH = Console.WindowWidth;

            ticksSinceLastScreenResize = 0;

            OnResizeTerminal();
        }

    HandlePendingComponentInput(); UpdateTabOverheadPanel();
    }

    private static void OnResizeTerminal()
    {
        /*
        if (tickCount < 2 && !FRONTEND_ONLY) {
            // Modify.TogglePlayPause(); 

            SharedElements.GetNowPlayingViewButton().Click();
        }
        */

        // redraw everything
        RedrawCurrentlyPlaying();
        RedrawPlaybackDetails();

        DrawLineBorderThing(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 5);
        
        // reposition playlist view
        if (playlistView != null)
        {
            playlistView.yPos = Console.WindowHeight - 1 - BOTTOM_BAR_MARGIN_BOTTOM;
            playlistView.xPos = Console.WindowWidth - (Console.WindowWidth / 3) + 2;
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

        StaticUtilities.DrawVerticalLineDivisor((Console.WindowWidth / 3), Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 4);
        StaticUtilities.DrawVerticalLineDivisor((Console.WindowWidth / 3) * 2, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 4);

        Console.SetCursorPosition((Console.WindowWidth / 3) + BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 3);
        Console.Write(ANSI_GRAY + "Next song:" + ANSI_RESET);

        nextSongLabel = null; // to redraw

        // Take care of rerendering tabs
        userLibListView!.entiresToDisplay = (Console.WindowHeight / 2) - 4;
        DrawTabContent(redraw: true);
    }

    private static string curContextLabel = "";
    private static string curAlbumLabel = "";
    private static void RedrawPlaybackContext(bool forceRedrawAlbum = true)
    {
        Console.SetCursorPosition((Console.WindowWidth / 3) + BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 2);
        int x = Console.CursorLeft;
        int y = Console.CursorTop;

        string[] retval = Read.GetCurrentPlaybackContext();
        var ctx = StaticUtilities.Trunacate(retval[0], (Console.WindowWidth / 3) - 6);
        var album = StaticUtilities.Trunacate(retval[1], (Console.WindowWidth / 3) - 9);

        // check if there's a difference between known context and actual context
        if (ctx != curContextLabel)
        {
            StaticUtilities.ClearRow(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 2, (Console.WindowWidth / 3) + 1, (Console.WindowWidth / 3) - 1);
            Console.Write(ANSI_GRAY + ctx + ANSI_RESET);
        }

        if (forceRedrawAlbum)
        {
            StaticUtilities.ClearRow(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 1, (Console.WindowWidth / 3) + 1, (Console.WindowWidth / 3) - 1);
            Console.SetCursorPosition(x, y + 1);
            Console.Write(ANSI_GRAY + "on " + ANSI_RESET + album);
        } 

        else
        {
            if (album != curAlbumLabel)
            {
                StaticUtilities.ClearRow(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 1, (Console.WindowWidth / 3) + 1, Console.WindowWidth / 3);
                Console.SetCursorPosition(x, y + 1);
                Console.Write(ANSI_GRAY + "on " + ANSI_RESET + album);
            }
        }

        curContextLabel = ctx;
        curAlbumLabel = album;
    }

    private static void DrawNextSongDetails()
    {
HandlePendingComponentInput();

        string? data = Read.GetNextSong();

HandlePendingComponentInput();

        if (data != nextSongLabel && data != null)
        {
            Console.SetCursorPosition((Console.WindowWidth / 3) + BOTTOM_BAR_MARGIN_LEFT, Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 2);
            StaticUtilities.ClearRow(Console.WindowHeight - BOTTOM_BAR_MARGIN_BOTTOM - 2, (Console.WindowWidth / 3) + BOTTOM_BAR_MARGIN_LEFT - 1, (Console.WindowWidth / 3) - 2);

HandlePendingComponentInput();

            Console.Write(ANSI_GRAY + StaticUtilities.Trunacate(data, (Console.WindowWidth / 3) - 6) + ANSI_RESET);
        }

HandlePendingComponentInput();
        nextSongLabel = data;
    }

    public static void DrawNotificationLabel(string s)
    {
        StaticUtilities.ClearRow(1);
        Console.SetCursorPosition(2, 1);
        Console.Write(ANSI_GRAY + s + ANSI_RESET); 
    }

    private static void UpdateTabOverheadPanel()
    {
        Console.SetCursorPosition(2, 3);
        
        for (int i = 0; i < 3; i++)
        {
            if ((Tab)i != tabState)
            {
                Console.Write(ANSI_GRAY);
            }

            Console.Write((Tab)i + " / ");

            if ((Tab)i != tabState)
            {
                Console.Write(ANSI_RESET);
            }
        }
    }

    private static void DrawTabContent(bool redraw = false)
    {
        if (tabState == Tab.Library)
        {
            if (components.Contains(userLibListView!) == false)
            {
                components.Add(userLibListView!);
                FOCUSED = userLibListView;
                userLibListView!.UpdateLabel();
            }

            // handle "Made for you" section -- get Discover Weekly and Release Radar playlists
            Task<SearchResponse>? discoverWeeklySearch = spotify?.Search.Item(
                new SearchRequest(SearchRequest.Types.Playlist, "discover weekly")  
            );

            Task<SearchResponse>? releaseRadarSearch = spotify?.Search.Item(
                new SearchRequest(SearchRequest.Types.Playlist, "releaseRadar")
            );

            var relaseRadar = releaseRadarSearch!.Result.Playlists.Items!.FirstOrDefault();
            var discoverWeekly = discoverWeeklySearch!.Result.Playlists.Items!.FirstOrDefault();

            userLibListView!.libData!.Add(new KeyValuePair<string, string>(relaseRadar!.Uri, "Made for you - " + relaseRadar!.Name));
            userLibListView.libData.Add(new KeyValuePair<string, string>(discoverWeekly!.Uri, "Made for you - " + discoverWeekly!.Name));
         
            // now, user playlists
            foreach(var playlist in allUserSavedPlaylists)
            {
                userLibListView.libData.Add(new KeyValuePair<string, string>(playlist.Key, "🗀 - " + playlist.Value));
            }

            if (redraw) userLibListView.UpdateLabel();
        }

        else if (tabState == Tab.Tracklist)
        {
            // start from coordinates (2, 5)

            components.Remove(userLibListView!);
            FOCUSED = playlistView;

            for (int i = 0; i < Console.WindowHeight - 12; i++)
            {
                Console.SetCursorPosition(2, 3 + i);
                Console.Write(new string(' ', Console.WindowWidth - 1));
            }

            Console.SetCursorPosition(2, 5);
            Console.Write("THIS IS THE TRACKLIST TAB AHSDKFHJASKDFH");

            for (int i = 0; i < tracklist.Count; i++)
            {
                Console.SetCursorPosition(2, 6 + i);
                Console.Write(tracklist[i].name + " on " + tracklist[i].album);
            }
        }

        else if (tabState == Tab.Search) 
        {
            components.Remove(userLibListView!);
            FOCUSED = searchInputField;

            for (int i = 0; i < Console.WindowHeight - 12; i++)
            {
                Console.SetCursorPosition(2, 3 + i);
                Console.Write(new string(' ', Console.WindowWidth - 1));
            }

            Console.SetCursorPosition(2, 5);
            Console.Write("Search: ");
        }
    }

    public static void UpdateAndOpenTracklistView(List<TracklistItem> newData)
    {
        tracklist = newData;
        tabState = Tab.Tracklist;
        PENDING_UPDATE_TAB_CONTENT = true;

        components.Remove(userLibListView);
        FOCUSED = null;
    }
}