import textual.widgets as w
from textual.app import App, ComposeResult
import os
from textual.color import Color
import textual
from textual.containers import Container, Grid
from textual.binding import Binding

import modification as m
import asyncio
import read as r

from utility import log

PERFORM_ASSERTS = False

"""
Key bindings:
space: toggle play/pause
left arrow: go to previous song
right arrow: go to next song

f: toggle heart song
r: change repeat setting
s: toggle shuffle
"""

# Adjust tick speed and slow tick speed *IN SECONDS*
TICK_SPEED = 0.1
SLOW_TICK_SPEED = 5

ONLY_FRONTEND = __name__ == "__main__"

# move this to utility.py later
def jassert(condition: bool):
    if PERFORM_ASSERTS:
        assert condition

# Custom widgets
class CurrentlyPlaying(w.Static):
    def compose(self) -> ComposeResult:
        yield w.Label("Currently playing goes here", id="currently_playing")

# contains info about shuffle and repeat
class PlaybackConfig(w.Static):
    def compose(self) -> ComposeResult:
        yield w.Label("Shuffle/repeat status info goes here", id="playback_config")

class SongProgressBar(w.Static):
    def compose(self) -> ComposeResult:
        yield w.ProgressBar(total=100, show_eta=False,
                            show_percentage=False, id="song_progress_bar")

class AddToPlaylist(w.Static):
    playlists = None

    def compose(self) -> ComposeResult:
        if not ONLY_FRONTEND:
            self.playlists = r.auth.user_playlists(r.auth.me()["id"])["items"]

            yield w.ListView(
                * [w.ListItem(w.Label(p["name"])) for p in self.playlists],
                id="add_to_playlist_list_view",
            )
        else:
            yield w.ListView(
                w.ListItem(w.Label("playlist 1")),
                w.ListItem(w.Label("playlist 2")),
                w.ListItem(w.Label("playlist 3")),
                w.ListItem(w.Label("playlist 4")),
                w.ListItem(w.Label("playlist 5")),
                id="add_to_playlist_list_view",
            )

# Main app
class SpotifyCLI(App):
    # elements
    currentlyPlaying = None
    playbackConfig = None
    songProgressBar = None
    statusLabel = None

    addToPlaylistListView = None
    
    # other
    CSS_PATH = "main.tcss"
    playlistsWithUri = {}

    PAUSED = False

    def on_mount(self) -> None:
        self.currentlyPlaying = self.query_one("#currently_playing", w.Label)
        self.playbackConfig = self.query_one("#playback_config", w.Label)
        # self.currentlyPlaying.styles.margin = (int(os.get_terminal_size()[1] - 10), 0, 0, 0)

        self.songProgressBar = self.query_one("#song_progress_bar", w.ProgressBar)
        self.songProgressBar.progress = 50

        self.statusLabel = self.query_one("#status_label", w.Label)

        self.addToPlaylistListView = self.query_one("#add_to_playlist_list_view", w.ListView)

        if not ONLY_FRONTEND:
            playlists = r.auth.user_playlists(r.auth.me()["id"])["items"]
            for p in playlists:
                self.playlistsWithUri[p["name"]] = p["uri"]


        asyncio.create_task(self.tick())
        asyncio.create_task(self.slowTick())


    def compose(self) -> ComposeResult:
        ##yield CurrentlyPlaying()
        ##yield SongProgressBar()
        yield AddToPlaylist()

        yield Container(
            w.Label("status label", id="status_label"),
            
            CurrentlyPlaying(),
            PlaybackConfig(),
            SongProgressBar(),
            id="current_playback_details_container"
        )

    # callbacks
    def on_checkbox_changed(self, changed: w.Checkbox.Changed):
        # `changed` is like a copy of the variable of the checkbox that was just changed.
        # `changed.control` is like a POINTER to the ACTUAL CHECKBOX that was just changed.
        pass

    # https://github.com/Textualize/textual/discussions/1840
    def on_list_view_selected(self, event: w.ListView.Selected):
        if event.item.parent.id == self.addToPlaylistListView.id:
            # Get current playlback with spotipy to get the track ID
            # then use spotipy again to add that track to playlist
            currentTrackID = r.auth.current_playback()["item"]["id"]
            playlistID = str(self.playlistsWithUri[str(event.item.children[0].renderable)])

            r.auth.playlist_add_items(playlistID, ["spotify:track:" + currentTrackID])

            self.statusLabel.refresh()

    async def on_key(self, key):
        val = str(key.key).lower()
        self.statusLabel.update(f"pressed {val}")
        
        # Toggle heart current song
        if val == "f":
            self.statusLabel.update("Toggle hearting current song")
            m.toggleHeartCurrentSong()
        
        if val == "r":
            self.statusLabel.update("Changing repeat setting")
            m.switchRepeat()
        
        if val == "s":
            self.statusLabel.update("Changing shuffle")
            m.toggleShuffle()

        # Play/pause, skip forward/backward
        if val == "space":
            m.togglePlayPause()
        if val == "left":
            m.skipBack()
        if val == "right":
            m.skipForward()

        await asyncio.sleep(.05)
        self.updateAllPlaybackInfo()


    # Utiliies
    def updateAllPlaybackInfo(self):
        self.currentlyPlaying.update(str(r.currentPlayback()))
        self.currentlyPlaying.refresh()

        self.playbackConfig.update(str(r.currentPlaybackConfig()))
        self.playbackConfig.refresh()

    def updateProgressbar(self):
        # self.songProgressBar.progress = r.getSongProgress()

        try:
            self.songProgressBar.progress = r.getSongProgress()
            self.songProgressBar.refresh()
        except Exception as e: 
            log("exception with progress bar: ")
            log(str(e))

    def centerProgressBar(self):
        self.songProgressBar.styles.margin = (0, 0, 0, int((os.get_terminal_size()[0] // 2) - 16  ))
        pass

    async def slowTick(self):
        """
        called frequently, but not as often as tick();
        use slowTick() for other not very important tasks
        """
        while True:
            self.centerProgressBar()
            await asyncio.sleep(SLOW_TICK_SPEED)

    async def tick(self):
        """
        called very frequently to update the TUI
        """
        while True:
            self.updateAllPlaybackInfo()
            self.updateProgressbar()
            await asyncio.sleep(TICK_SPEED)


def init():
    app = SpotifyCLI()
    app.run()

if __name__ == "__main__": init()