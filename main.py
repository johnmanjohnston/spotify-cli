from selenium import webdriver
from selenium.webdriver.common.by import By
from time import sleep
import pyautogui as pag

import modification as m
import auth
import read
import utility

sp = auth.authenticateUser()

options = webdriver.ChromeOptions()

options.add_argument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data")
options.add_argument("profile-directory=Default")

driver = webdriver.Chrome(options=options)

m.driver = driver
read.auth = sp

driver.get("https://open.spotify.com/")

sleep(5)

utility.alt_tab()

while True:
    a = input("1 - play/pause; 2 - skip back; 3 - skip forward ")
    if a == "1": m.togglePlayPause()
    if a == "2": m.skipBack()
    if a == "3": m.skipForward()

    read.a()