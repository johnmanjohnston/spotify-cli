import textual.widgets as w
from textual.app import App, ComposeResult
import os
from textual.color import Color
import textual
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
# Main app
class SpotifyCLI(App):
    currentlyPlaying = None

    CSS_PATH = "main.tcss"

    def on_mount(self) -> None:
        jassert(r.auth != None)
        jassert(m.driver != None)

        self.currentlyPlaying = self.query_one("#currently_playing", w.Label)
        self.currentlyPlaying.styles.margin = (int(os.get_terminal_size()[1] - 10), 0, 0, 0)

        asyncio.create_task(self.tick())


    def compose(self) -> ComposeResult:
        yield CurrentlyPlaying()

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

    async def tick(self):
        while True:
            self.updateCurrentPlayback()
            await asyncio.sleep(.1)


def init():
    app = SpotifyCLI()
    app.run()

if __name__ == "__main__": init()