import spotipy

auth: spotipy.Spotify = None # set in main.py

def currentPlayback():
    if auth == None: return "AUTH IS NOT CONFIGURED YOU BUFFOON"

    data = auth.current_playback()
    if data == None: return False

    name = data["item"]["name"]
    mainArtist = data["item"]["artists"][0]["name"]

    retval = [
        name,
        mainArtist,
        data["is_playing"]
    ]

    print(retval)
    return retval # I FORGOT I HAVE TO *RETURN* THIS KILL ME

if __name__ == "__main__":
    import sys
    import auth as a
    auth = a.authenticateUser()
    eval(f"{sys.argv[1]}()")