import spotipy
from utility import log
import selenium.types
import sharedelements

auth: spotipy.Spotify = None # set in main.py

def currentPlaybackConfig():
    """
    Returns info about repeat and shuffle

    WARNING: SHUFFLE AND REPEAT BUTTONS CAN SOMETIMES BE DISABLED, 
    TODO: IMPLEMENT DISABLED CHECKS TO SHUFFLE AND REPEAT BUTTONS
    """
    try:
        isShuffle = "on" if sharedelements.getShuffleButton().get_attribute("aria-checked").lower() == "true" else "off"
        repeatState = None

        match sharedelements.getRepeatButton().get_attribute("aria-checked"):
            case "mixed":
                repeatState = "song"
            case "true":
                repeatState = "list"
            case "false":
                repeatState = "none"

        return f"Shuffle: {isShuffle} - Repeat: {repeatState}"
    except Exception as e:
        return "Loading shuffle/repeat status..."

def currentPlayback():
    """
    Returns name of song, name of main artist, and wether 
    the song is hearted or not
    """
    heartedChar = "♥"
    unheartedChar = "♡"

    try:
        isHearted = sharedelements.getHeartButton().get_attribute("aria-checked").lower() == "true"
        retval = f'{sharedelements.getSongNameLink().text} - {sharedelements.getMainArtistLink().text} {heartedChar if isHearted else unheartedChar}'
        return retval
    except Exception as e:
        log(str(e))
        return 'driver not assigned. check sharedelements.py' if sharedelements.driver == None else 'Loading current playback...'

def getSongProgress() -> float:
    try:
        progressBarStyle: str = sharedelements.getProgressBarDiv().get_attribute("style")
        retval: float = float(progressBarStyle.split(": ")[1].split("%")[0])
        return retval
    except Exception as e:
        log("ERROR")
        log(str(e))
        return 0

def testAuth():
    """playlists = auth.user_playlists(auth.me()['id'])
    # playlist name: (playlists["items"][i]["name"]) where 'i' is the index

    for i in range(len(playlists["items"])):
        print(playlists["items"][i]["name"])
        print(playlists["items"][i]["id"])
        print()"""
    
    print(auth.current_playback()["item"]["id"])

if __name__ == "__main__":
    import sys
    import auth as a
    auth = a.authenticateUser()
    eval(f"{sys.argv[1]}()")