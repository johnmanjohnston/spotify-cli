import requests # to get cover images
import climage # to display cover images
import spotipy

def getCurrentSongData(sp: spotipy.Spotify) -> dict:
    currentSongData = sp.current_playback()["item"]["album"]
    coverImage = (sp.current_playback()["item"]["album"]["images"][1]["url"])
    name = currentSongData["name"]
    mainArtist = currentSongData["artists"][0]["name"]

    return {
        "name": name,
        "main_artist": mainArtist
    }

def main(sp: spotipy.Spotify):
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