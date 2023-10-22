from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.remote.webelement import WebElement

driver: webdriver.Chrome = None

def getHeartButton() -> WebElement:
    return driver.find_element(By.XPATH, f"//button[@data-testid='add-button']")

def getRepeatButton() -> WebElement:
    return driver.find_element(By.XPATH, "//button[@data-testid='control-button-repeat']")

def getShuffleButton() -> WebElement:
    return driver.find_element(By.XPATH, f"//button[@data-testid='control-button-shuffle']")

def getHeartButton() -> WebElement:
    return driver.find_element(By.XPATH, f"//button[@data-testid='add-button']")

def getSongNameLink():
    return driver.find_element(By.XPATH, f"//a[@data-testid='context-item-link']")

def getMainArtistLink():
    return driver.find_element(By.XPATH, f"//a[@data-testid='context-item-info-artist']")

def getProgressBarDiv():
    return driver.find_element(By.XPATH, f"//div[@data-testid='progress-bar']")

def getPlayPauseButton():
    return driver.find_element(By.XPATH, f"//button[@data-testid='control-button-playpause']")

def getSkipBACKbutton():
    return driver.find_element(By.XPATH, f"//button[@data-testid='control-button-skip-back']")

def getSkipFORWARDbutton():
    return driver.find_element(By.XPATH, f"//button[@data-testid='control-button-skip-forward']")