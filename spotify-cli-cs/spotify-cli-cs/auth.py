from tokens import *
import spotipy
from spotipy.oauth2 import SpotifyOAuth

ACCESS_TOKEN_SAVE_DIR = "C:/Users/USER/OneDrive/Desktop/nerd/spotify-cli/spotify-cli-cs/spotify-cli-cs"

authManager = SpotifyOAuth(client_id=CLIENT_ID,
                                            client_secret=CLIENT_SECRET,
                                            redirect_uri="https://example.com",
                                            scope=
"user-library-read user-read-playback-state user-modify-playback-state playlist-read-private playlist-read-collaborative playlist-modify-public playlist-modify-private")

def authenticateUser() -> spotipy.Spotify:
    sp = spotipy.Spotify(auth_manager=authManager)
    return sp

if __name__ == "__main__":
    a = authenticateUser()
    a.current_user()

    accessToken = authManager.get_access_token(as_dict=False)
    # print("Recieved access token", accessToken)

    with open(f"{ACCESS_TOKEN_SAVE_DIR}/access_token", "w") as f:
        f.write(accessToken)
    f.close

    print("logged in as", a.current_user()["display_name"])

    print("Access token written")
