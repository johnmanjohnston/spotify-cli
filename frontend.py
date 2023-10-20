import textual.widgets as w
from textual.app import App, ComposeResult

import modification as m
import asyncio
import read as r

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
        asyncio.create_task(self.tick())


    def compose(self) -> ComposeResult:
        yield CurrentlyPlaying()

    def on_key(self, key):
        val = key.key
        
        if val == "space":
            m.togglePlayPause()
        if val == "left":
            m.skipBack()
        if val == "right":
            m.skipForward()

    async def tick(self):
        while True:
            # Update currently playing
            if (r.currentPlayback() == False):
                self.currentlyPlaying.update(str("No song"))
            else:
                self.currentlyPlaying.update(str (r.currentPlayback()[0]) + " - " + str (r.currentPlayback()[1]))
            self.currentlyPlaying.refresh()

            await asyncio.sleep(.5)


def init():
    app = SpotifyCLI()
    app.run()

if __name__ == "__main__": init()