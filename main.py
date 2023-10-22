from selenium import webdriver
import selenium.common.exceptions
from time import sleep

import modification as m
import auth
import read
import utility
import frontend
import sharedelements

# auth user
sp = auth.authenticateUser()

# configure webdriver
options = webdriver.ChromeOptions()

options.add_argument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data")
options.add_argument("profile-directory=Default")

try:
    driver = webdriver.Chrome(options=options)
except selenium.common.exceptions.SessionNotCreatedException:
    print("YOU BUFFOON CLOSE OTHER INSTANCES OF CHROME")

# configure the driver for modificiation, and the authentication for reading
read.auth = sp
sharedelements.driver = driver

print("Performing assertions...")

assert sharedelements.driver != None
assert read.auth != None

print("Assertions passed")

# open Spotify, wait, tab out, and initialize the frontend
driver.get("https://open.spotify.com/")
sleep(3)

utility.alt_tab()

sleep(5)
frontend.init()