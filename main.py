import requests
from tokens import *


def getAccessToken(clientID, clientSecret) -> str:
    """
    We need an access token for Spotify to work. Otherwise Spotify 
    will get cocky and go 'uHm sOrRy yOu nEeD pReMiUm'
    """
    payload = {
        "grant_type": "client_credentials",
        "client_id": clientID,
        "client_secret": clientSecret,
        "scope": "user-read-playback-state"
    }

    headers = {
        "Content-Type": "application/x-www-form-urlencoded"
    }

    url = "https://accounts.spotify.com/api/token"

    res = requests.post(url, data=payload, headers=headers)

    import json
    jsonData = json.loads(res.text)
    return jsonData["access_token"]

def spotifyAPICall(token, route):
    headers = {
        "Authorization": f"Bearer {token}",
    }

    url = f"https://api.spotify.com/v1/{route}"

    res = requests.get(url, headers=headers)
    import json
    jsonData = json.loads(res.text)
    return jsonData

ACCCESS_TOKEN = getAccessToken(CLIENT_ID, CLIENT_SECRET)
print(ACCCESS_TOKEN)

currentPlaybackData = spotifyAPICall("BQBkJ1LQL2xWlA2MjMR0IlfaliNgmWnAiNEx7F8yYETfkmgP-XssjNvO2B6mtQVVoi5Al4aRo8ssU0qaE_2gB_kizMYnJvXtMKW4rTzb4K6HT2mYkYXIjaSjNyrep3zj6IIpEDSw-q-oW7eykNeWMQi27aj8Doidz2vXmV0pP_-bIZVRarhiT5Rx9swigcA08DtAYyS1ins0pJ4Wu9BFX52X", 
"me/player/")
print(currentPlaybackData["item"])