import spotipy

auth: spotipy.Spotify = None # set in main.py

def a():
    print(auth.current_user())