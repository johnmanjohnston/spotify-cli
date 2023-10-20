from selenium import webdriver
from time import sleep

import modification as m
import auth
import read
import utility
import frontend

# auth user
sp = auth.authenticateUser()

# configure webdriver
options = webdriver.ChromeOptions()

options.add_argument("user-data-dir=C:\\Users\\USER\\AppData\\Local\\Google\\Chrome\\User Data")
options.add_argument("profile-directory=Default")

driver = webdriver.Chrome(options=options)

# configure the driver for modificiation, and the authentication for reading
m.driver = driver
read.auth = sp

# open Spotify, wait, tab out, and initialize the frontend
driver.get("https://open.spotify.com/")
sleep(5)

utility.alt_tab()
frontend.init()