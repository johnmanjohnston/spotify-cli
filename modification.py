# Contains functions which MODIFY data for the user's Spotify client
from selenium import webdriver
from selenium.webdriver.common.by import By

driver: webdriver.Chrome = None # this is set in main.py

def togglePlayPause():
    pressButtonWithtestidAttribute("control-button-playpause")

def skipBack():
    pressButtonWithtestidAttribute("control-button-skip-back")

def skipForward():
    pressButtonWithtestidAttribute("control-button-skip-forward")

def toggleHeartCurrentSong():
    driver.find_element(By.XPATH, f"//button[@data-testid='add-button']").click()

# more of like a utility function?
def pressButtonWithtestidAttribute(testidAttribute: str):
    driver.find_element(By.XPATH, f"//button[@data-testid='{testidAttribute}']").click()