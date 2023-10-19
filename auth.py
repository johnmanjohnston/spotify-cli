from tokens import *
import spotipy
from spotipy.oauth2 import SpotifyOAuth
import read

def authenticateUser() -> spotipy.Spotify:
    sp = spotipy.Spotify(auth_manager=SpotifyOAuth(client_id=CLIENT_ID,
                                                client_secret=CLIENT_SECRET,
                                                redirect_uri="localhost:8000",
                                                scope="user-library-read user-read-playback-state user-modify-playback-state"))
    return sp
