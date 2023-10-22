# Contains functions which MODIFY data for the user's Spotify client
from selenium import webdriver
from selenium.webdriver.common.by import By
import sharedelements

def togglePlayPause():
    sharedelements.getPlayPauseButton().click()

def skipBack():
    sharedelements.getSkipBACKbutton().click()

def skipForward():
    sharedelements.getSkipFORWARDbutton().click()

def toggleHeartCurrentSong():
    sharedelements.getHeartButton().click()