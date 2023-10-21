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

PERFORM_ASSERTS = False

# move this to utility.py later
def jassert(condition: bool):
    if PERFORM_ASSERTS:
        assert condition

# Custom widgets
class CurrentlyPlaying(w.Static):
    def compose(self) -> ComposeResult:
        yield w.Label("Currently playing goes here", id="currently_playing")

class SongProgressBar(w.ProgressBar):
    def compose(self) -> ComposeResult:
        yield w.ProgressBar(total=100, show_eta=False, 
                            show_percentage=False, id="song_progress_bar")

# Main app
class SpotifyCLI(App):
    currentlyPlaying = None
    songProgressBar = None

    CSS_PATH = "main.tcss"

    def on_mount(self) -> None:
        jassert(r.auth != None)
        jassert(m.driver != None)

        self.currentlyPlaying = self.query_one("#currently_playing", w.Label)
        # self.currentlyPlaying.styles.margin = (int(os.get_terminal_size()[1] - 10), 0, 0, 0)

        self.songProgressBar = self.query_one("#song_progress_bar", w.ProgressBar)

        asyncio.create_task(self.tick())


    def compose(self) -> ComposeResult:
        ##yield CurrentlyPlaying()
        ##yield SongProgressBar()
        yield Container(
            CurrentlyPlaying(),
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

        self.updateCurrentPlayback()


    # Utiliies
    def updateCurrentPlayback(self):
        self.currentlyPlaying.update(str(r.currentPlayback()))
        self.currentlyPlaying.refresh()

    def updateProgressbar(self):
        # self.songProgressBar.progress = r.getSongProgress()

        try:
            self.songProgressBar.progress = r.getSongProgress()
            self.songProgressBar.refresh()
        except Exception as e: 
            log("exception with progress bar: ")
            log(str(e))

    async def tick(self):
        while True:
            self.updateCurrentPlayback()
            self.updateProgressbar()
            await asyncio.sleep(.1)


def init():
    app = SpotifyCLI()
    app.run()

if __name__ == "__main__": init()