from selenium import webdriver
from selenium.webdriver.common.by import By
from time import sleep
import pyautogui as pag

import modification as m
import utility

options = webdriver.ChromeOptions()

options.add_argument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data")
options.add_argument("profile-directory=Default")

driver = webdriver.Chrome(options=options)
driver.get("https://open.spotify.com/")

sleep(5)

utility.alt_tab()

while True:
    a = input("Toggle play pause")
    m.togglePlayPause(driver)
    