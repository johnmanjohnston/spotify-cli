from auth import auth
import climage
import requests

sp = auth()

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
coverAsText = climage.convert('cover.png', is_unicode=True, width=20)
print(coverAsText)