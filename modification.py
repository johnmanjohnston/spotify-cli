# Contains functions which MODIFY data for the user's Spotify client
from selenium import webdriver
from selenium.webdriver.common.by import By

def togglePlayPause(driver: webdriver.Chrome):
    """
    Presses the play/pause button at the Spotify webpage
    """
    driver.find_element(By.XPATH, "//button[@data-testid='control-button-playpause']").click()