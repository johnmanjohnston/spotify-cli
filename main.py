from selenium import webdriver
import selenium.common.exceptions
from time import sleep

import modification as m
import auth
import read
import utility
import frontend
import sharedelements

PERFORM_ASSERTIONS = True
def jassert(condition: bool):
    if PERFORM_ASSERTIONS: 
        assert condition

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

print("Performing initialization assertions...")

jassert(sharedelements.driver != None)
jassert(read.auth != None)

print("Assertions passed")

# open Spotify, wait, tab out, and initialize the frontend
driver.get("https://open.spotify.com/")

sleep(3)

utility.alt_tab()

print("Performing post-initialization assertions...")
# make sure that Spotify elements loaded. if any one element loads successfully, 
# we can infer that other elements loaded successfully too
jassert(sharedelements.getPlayPauseButton() != None)
print("Assertions passed")

sleep(2)
frontend.init()