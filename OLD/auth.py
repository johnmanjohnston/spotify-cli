from tokens import *
import spotipy
from spotipy.oauth2 import SpotifyOAuth

def auth() -> spotipy.Spotify:
    sp = spotipy.Spotify(auth_manager=SpotifyOAuth(client_id=CLIENT_ID,
                                                client_secret=CLIENT_SECRET,
                                                redirect_uri="https://example.com",
                                                scope="user-library-read user-read-playback-state user-modify-playback-state"))
    return sp

a = auth()
a.pause_playback()