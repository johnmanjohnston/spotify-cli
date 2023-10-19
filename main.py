from selenium import webdriver
from selenium.webdriver.common.by import By
from time import sleep
import pyautogui as pag

options = webdriver.ChromeOptions()

options.add_argument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data")
options.add_argument("profile-directory=Default")

driver = webdriver.Chrome(options=options)
driver.get("https://open.spotify.com/")

sleep(5)

"""
`driver.minimize_window()` doesn't let audio play for some reason.
As an alternative, we just Alt+Tab out of the Chrome instance
"""

pag.keyDown("alt")
pag.keyDown("tab")
pag.keyUp("alt")
pag.keyUp("tab")

while True:
    a = input("Toggle play pause")
    (driver.find_element(By.XPATH, "//button[@data-testid='control-button-playpause']")).click()
