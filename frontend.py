import textual.widgets as w
from textual.app import App, ComposeResult
import os
from textual.color import Color
import textual
from textual.containers import Container
from textual.binding import Binding

import modification as m
import asyncio
import read as r

from utility import log
import imgtotext

PERFORM_ASSERTS = False

"""
Key bindings:
space: toggle play/pause
left arrow: go to previous song
right arrow: go to next song

f: toggle heart song
r: change repeat setting (TO DO)
s: toggle shuffle (TO DO)
"""

# Adjust tick speed and slow tick speed *IN SECONDS*
TICK_SPEED = 0.1
SLOW_TICK_SPEED = 5

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

class SongProgressBar(w.ProgressBar):
    def compose(self) -> ComposeResult:
        yield w.ProgressBar(total=100, show_eta=False,
                            show_percentage=False, id="song_progress_bar")

# Main app
class SpotifyCLI(App):
    currentlyPlaying = None
    playbackConfig = None
    songProgressBar = None

    CSS_PATH = "main.tcss"

    def on_mount(self) -> None:
        jassert(r.auth != None)
        jassert(m.driver != None)

        self.currentlyPlaying = self.query_one("#currently_playing", w.Label)
        self.playbackConfig = self.query_one("#playback_config", w.Label)
        # self.currentlyPlaying.styles.margin = (int(os.get_terminal_size()[1] - 10), 0, 0, 0)

        self.songProgressBar = self.query_one("#song_progress_bar", w.ProgressBar)
        self.songProgressBar.progress = 50

        # margin = (terminal width / 2) - (width of progress bar / 2) 

        asyncio.create_task(self.tick())
        asyncio.create_task(self.slowTick())


    def compose(self) -> ComposeResult:
        ##yield CurrentlyPlaying()
        ##yield SongProgressBar()
        yield Container(
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

    def on_key(self, key):
        val = key.key
        
        # Toggle heart current song
        if val == "f":
            m.toggleHeartCurrentSong()

        # Play/pause, skip forward/backward
        if val == "space":
            m.togglePlayPause()
        if val == "left":
            m.skipBack()
        if val == "right":
            m.skipForward()

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