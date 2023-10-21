import spotipy
import selenium.webdriver
from selenium.webdriver.common.by import By
from utility import log

auth: spotipy.Spotify = None # set in main.py
driver: selenium.webdriver.Chrome # set in main.py

def getContentOftestidElement(elType, testIDValue):
    return driver.find_element(By.XPATH, f"//{elType}[@data-testid='{testIDValue}']").text

def currentPlayback():
    try:
        # context-item-info-artist
        retval = f'{getContentOftestidElement("a", "context-item-link")} - {getContentOftestidElement("a", "context-item-info-artist")}'
        return retval
    except Exception as e:
        log(str(e))
        return "Loading..."

if __name__ == "__main__":
    import sys
    import auth as a
    auth = a.authenticateUser()
    eval(f"{sys.argv[1]}()")