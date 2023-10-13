from auth import auth

sp = auth()

currentSongData = sp.current_playback()["item"]["album"]