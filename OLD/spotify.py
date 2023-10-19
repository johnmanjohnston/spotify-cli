import requests # to get cover images
# import climage # to display cover images ! WILL PROBABLY REMOVE THIS
import spotipy
import curses

import PIL # to iterate over cover image pixels
from PIL import Image
import numpy as np

from logger import log

import locale
locale.setlocale(locale.LC_ALL, '')

def getCurrentSongData(sp: spotipy.Spotify) -> dict:
    """
    returns the name, main artist of the song,
    and if the song is playing or not
    """
    currentSongData = sp.current_playback()["item"]
    name = currentSongData["name"]
    mainArtist = currentSongData["artists"][0]["name"]
    
    isPlaying = bool(sp.current_playback()["is_playing"])

    return {
        "name": name,
        "main_artist": mainArtist,
        "is_playing": isPlaying
    }

# stdscr.addstr(".█.", curses.color_pair(3))
def drawCurrentSongCover(sp: spotipy.Spotify, stdscr: curses.initscr):
    """
    curses doesn't support 'ANSI' or whatever 
    the hell it's called. this function is a 
    custom implimentation to draw images to 
    the terminal.
    """
    # stdscr.addstr(5, 5, "█", curses.color_pair(207))

    # save album cover
    coverImageURL = sp.current_playback()["item"]["album"]["images"][2]["url"]
    with requests.get(coverImageURL, stream=True) as r:
            r.raise_for_status()
            with open("cover.png", 'wb') as f:
                for chunk in r.iter_content(chunk_size=8192): 
                    if chunk: 
                        f.write(chunk)


    img = Image.open("cover.png")
    img.load()
    imgData = np.asarray(img, dtype="int32")

    RENDER_DIMENSIONS = 32
    IMAGE_DIMENSIONS = img.width # height and width are same

    log(str(IMAGE_DIMENSIONS))

    for y in range(RENDER_DIMENSIONS // 2):
        for x in range(RENDER_DIMENSIONS ):
            # stdscr.addstr(y, x, "█", curses.color_pair(imgData[x+1, y+1][0]))
            # TODO: RENDER CHARACTER BY ACCESS INDEX OF imgData OR SMTH IDK
            pass

def main(sp: spotipy.Spotify):
    stdscr.addstr(0, 0, mystring.encode('UTF-8'))
    currentSongData = sp.current_playback()["item"]["album"]

    print()
    print(currentSongData)

    coverImage = (sp.current_playback()["item"]["album"]["images"][1]["url"])

    def download_file(url):
        with requests.get(url, stream=True) as r:
            r.raise_for_status()
            with open("cover.png", 'wb') as f:
                for chunk in r.iter_content(chunk_size=8192): 
                    if chunk: 
                        f.write(chunk)
                        
    download_file(coverImage)
    coverAsText = climage.convert('cover.png', is_unicode=True, width=30)
    print(coverAsText)