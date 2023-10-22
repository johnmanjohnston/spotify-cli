import spotipy
from utility import log
import selenium.types
import sharedelements

auth: spotipy.Spotify = None # set in main.py

def currentPlaybackConfig():
    """
    Returns info about repeat and shuffle
    """
    try:
        isShuffle = sharedelements.getShuffleButton().get_attribute("aria-checked").lower() == "true"
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
        return 'driver/auth not assigned (check read.py for auth, and sharedelements.py for driver)' if auth == None else 'Loading current playback...'

def getSongProgress() -> float:
    try:
        progressBarStyle: str = sharedelements.getProgressBarDiv().get_attribute("style")
        retval: float = float(progressBarStyle.split(": ")[1].split(".")[0])
        return retval
    except Exception as e:
        log("ERROR")
        log(e)
        return 50

if __name__ == "__main__":
    import sys
    import auth as a
    auth = a.authenticateUser()
    eval(f"{sys.argv[1]}()")