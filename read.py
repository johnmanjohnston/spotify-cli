import spotipy
import selenium.webdriver
from selenium.webdriver.common.by import By
from utility import log
import selenium.types

auth: spotipy.Spotify = None # set in main.py
driver: selenium.webdriver.Chrome # set in main.py

def getContentOftestidElement(elType, testIDValue):
    return driver.find_element(By.XPATH, f"//{elType}[@data-testid='{testIDValue}']").text

def currentPlaybackConfig():
    """
    Returns info about repeat and shuffle
    """
    try:
        isShuffle = driver.find_element(By.XPATH, f"//button[@data-testid='control-button-shuffle']").get_attribute("aria-checked").lower() == "true"
        repeatState = None

        match driver.find_element(By.XPATH, "//button[@data-testid='control-button-repeat']").get_attribute("aria-checked"):
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
        isHearted = driver.find_element(By.XPATH, f"//button[@data-testid='add-button']").get_attribute("aria-checked").lower() == "true"

        retval = f'{getContentOftestidElement("a", "context-item-link")} - {getContentOftestidElement("a", "context-item-info-artist")} {heartedChar if isHearted else unheartedChar}'
        return retval
    except Exception as e:
        log(str(e))
        return 'driver/auth not assigned (check read.py)' if auth == None or driver == None else 'Loading current playback...'

def getSongProgress() -> float:
    try:
        progressBarStyle: str = driver.find_element(By.XPATH, "//div[@data-testid='progress-bar']").get_attribute("style")
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