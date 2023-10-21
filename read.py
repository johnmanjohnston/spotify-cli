import spotipy
import selenium.webdriver
from selenium.webdriver.common.by import By
from utility import log
import selenium.types

auth: spotipy.Spotify = None # set in main.py
driver: selenium.webdriver.Chrome # set in main.py

def getContentOftestidElement(elType, testIDValue):
    return driver.find_element(By.XPATH, f"//{elType}[@data-testid='{testIDValue}']").text

def currentPlayback():
    heartedChar = "♥"
    unheartedChar = "♡"

    try:
        isHearted = driver.find_element(By.XPATH, f"//button[@data-testid='add-button']").get_attribute("aria-checked").lower() == "true"

        retval = f'{getContentOftestidElement("a", "context-item-link")} - {getContentOftestidElement("a", "context-item-info-artist")} {heartedChar if isHearted else unheartedChar}'
        return retval
    except Exception as e:
        log(str(e))
        return "Loading..."

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