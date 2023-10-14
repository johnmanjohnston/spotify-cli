import requests # to get cover images
import climage # to display cover images
import spotipy

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

def getCurrentSongCover(sp: spotipy.Spotify) -> str:
    coverImage = (sp.current_playback()["item"]["album"]["images"][1]["url"])
    coverAsText = climage.convert('cover.png', is_unicode=False, width=15)
    with open("a.txt", "w") as f:
        f.write(coverAsText)
        f.close()

    return coverAsText

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