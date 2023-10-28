using spotify_cli_cs;
using Terminal.Gui;
using OpenQA.Selenium.Chrome;
using SpotifyAPI.Web;
using System;
using SpotifyAPI.Web.Auth;
using System.IO;
using System.Threading;
using OpenQA.Selenium;

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

        Application.Init();
        Colors.Base.Normal = Application.Driver.MakeAttribute(Color.Green, Color.Black);
        try {
            Application.Run(new MyView());
        } finally {
            Application.Shutdown();
        }
    }   
}

/*

return;

Application.Init();

try
{
    Application.Run(new MyView());
}
finally
{
    Application.Shutdown();
}
*/